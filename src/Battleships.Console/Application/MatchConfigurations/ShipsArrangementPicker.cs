using Battleships.Console.Application.Fleets;

namespace Battleships.Console.Application.MatchConfigurations;

public static class ShipsArrangementPicker
{
    public delegate CoordinatesSet ShipArrangementPicker(IReadOnlyList<CoordinatesSet> shipArrangements);
    public delegate IReadOnlyCollection<CoordinatesSet> ShipArrangementsProvider(int shipIndex);
    public static CoordinatesSet[] PickShipsArrangement(CoordinatesSet[] ships, ShipArrangementPicker shipArrangementPicker, ShipArrangementsProvider shipArrangementsProvider)
    {
        var stack = new Stack<Node>();
        
        var currentNode = Node.CreateRootNode();
        stack.Push(currentNode);

        while (stack.Count > 0)
        {
            currentNode = stack.Peek();
            
            var notOverlappingShipArrangements = currentNode.GetNotOverlappingShipArrangements(shipArrangementsProvider);

            if (notOverlappingShipArrangements.Count == 0)
            {
                currentNode.SelfExcludeFromParent();

                stack.Pop();
                continue;
            }
            
            var shipArrangement = shipArrangementPicker(notOverlappingShipArrangements);
            
            var nextNode = Node.Create(currentNode, shipArrangement);

            if (nextNode.ShipsArrangement.Length == ships.Length)
            {
                return nextNode.ShipsArrangement;
            }
            
            stack.Push(nextNode);
        }

        return currentNode.ShipsArrangement;
    }
    
    private class Node
    {
        private readonly List<CoordinatesSet> _excluded = new();
        private readonly Node? _parent;
        private readonly CoordinatesSet? _currentShipArrangement;
        private readonly int _currentShipIndex;

        public CoordinatesSet[] ShipsArrangement { get; }

        private Node(Node? parent, CoordinatesSet? currentShipArrangement, CoordinatesSet[] shipsArrangement, int currentShipIndex)
        {
            _parent = parent;
            ShipsArrangement = shipsArrangement;
            _currentShipArrangement = currentShipArrangement;
            _currentShipIndex = currentShipIndex;
        }
        
        public static Node Create(Node parent, CoordinatesSet currentShipArrangement) => 
            new(parent, 
                currentShipArrangement, 
                parent.ShipsArrangement.Concat(new[] { currentShipArrangement }).ToArray(), 
                parent._currentShipIndex+1);

        public static Node CreateRootNode() => new(
            null, 
            null, 
            Array.Empty<CoordinatesSet>(),
            0);

        public List<CoordinatesSet> GetNotOverlappingShipArrangements(
            ShipArrangementsProvider shipArrangementsProvider)
        {
            var result = shipArrangementsProvider(_currentShipIndex)
                .Where(x => !_excluded.Contains(x))
                .Where(x => !CoordinatesSet.AreSomeOverlapping(
                    ShipsArrangement.Concat(new[] { x }).ToArray()))
                .ToList();
            
            return result;
        }

        public void SelfExcludeFromParent()
        {
            if (_currentShipArrangement is not null)
            {
                _parent?._excluded.Add(_currentShipArrangement);
            }
        }
    }
}