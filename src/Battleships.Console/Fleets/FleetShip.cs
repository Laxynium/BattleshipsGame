namespace Battleships.Console.Fleets;

public record FleetShip
{
    private readonly Dictionary<Coordinate, bool> _coordinates;

    private FleetShip(IEnumerable<Coordinate> coordinates) => 
        _coordinates = coordinates.ToDictionary(x=>x, _=> false);

    public static FleetShip Create(Coordinate coordinate, params Coordinate[] coordinates)
    {
        var allCoordinates = new[] { coordinate }.Concat(coordinates).ToHashSet();

        if (!AreCoordinatesConnected(allCoordinates))
        {
            throw new FleetShipCoordinatesAreDisconnected();
        }
        
        return new FleetShip(allCoordinates);
    }

    public ShootResult ReceiveShot(Coordinate coordinate)
    {
        if (!_coordinates.ContainsKey(coordinate))
        {
            return ShootResult.Miss;
        }

        _coordinates[coordinate] = true;

        if (_coordinates.Values.All(x => x == true))
        {
            return ShootResult.Sunk;
        }

        return ShootResult.Hit;
    }

    public bool IsOverlappingWith(FleetShip another) => 
        _coordinates.Keys.ToHashSet().Overlaps(another._coordinates.Keys);

    private static bool AreCoordinatesConnected(IReadOnlyCollection<Coordinate> allCoordinates)
    {
        var connectedCoordinates = new List<Coordinate> {allCoordinates.First()};
        var queue = new Queue<Coordinate>(connectedCoordinates);
        while (queue.Count > 0)
        {
            var coordinate = queue.Dequeue();

            var nextCoordinatesToVisit = coordinate
                .GetNeighbours()
                .Intersect(allCoordinates)
                .Except(connectedCoordinates)
                .ToList();
            
            connectedCoordinates.AddRange(nextCoordinatesToVisit);
            
            foreach (var nextCoordinate in nextCoordinatesToVisit)
            {
                queue.Enqueue(nextCoordinate);
            }
        }

        return connectedCoordinates.Count == allCoordinates.Count;
    }
}