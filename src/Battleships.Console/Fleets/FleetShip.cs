namespace Battleships.Console.Fleets;

public record FleetShip
{
    private readonly Dictionary<Coordinates, bool> _holes;

    private FleetShip(IEnumerable<Coordinates> coordinates) => 
        _holes = coordinates.ToDictionary(x=>x, _=> false);

    public static FleetShip Create(Coordinates head, params Coordinates[] tail)
    {
        var allCoordinates = new[] { head }.Concat(tail).ToHashSet();

        if (!AreCoordinatesConnected(allCoordinates))
        {
            throw new FleetShipCoordinatesAreDisconnectedException();
        }
        
        return new FleetShip(allCoordinates);
    }

    public ShootResult ReceiveShot(Coordinates coordinate)
    {
        if (!_holes.ContainsKey(coordinate))
        {
            return ShootResult.Miss;
        }

        _holes[coordinate] = true;

        return IsSunk() ? ShootResult.Sunk : ShootResult.Hit;
    }

    public bool IsOverlappingWith(FleetShip another) => 
        _holes.Keys.ToHashSet().Overlaps(another._holes.Keys);

    public bool IsSunk() => 
        _holes.Values.All(x => x == true);

    private static bool AreCoordinatesConnected(IReadOnlyCollection<Coordinates> coordinatesSet)
    {
        var connectedCoordinatesSets = new List<Coordinates> {coordinatesSet.First()};
        var queue = new Queue<Coordinates>(connectedCoordinatesSets);
        while (queue.Count > 0)
        {
            var coordinate = queue.Dequeue();

            var nextCoordinatesToVisit = coordinate
                .GetNeighbourhood()
                .Intersect(coordinatesSet)
                .Except(connectedCoordinatesSets)
                .ToList();
            
            connectedCoordinatesSets.AddRange(nextCoordinatesToVisit);
            
            foreach (var nextCoordinate in nextCoordinatesToVisit)
            {
                queue.Enqueue(nextCoordinate);
            }
        }

        return connectedCoordinatesSets.Count == coordinatesSet.Count;
    }
}