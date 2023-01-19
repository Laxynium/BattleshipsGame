namespace Battleships.Console.Fleets;

public record FleetShip
{
    private readonly HashSet<Coordinate> _coordinates;

    private FleetShip(IEnumerable<Coordinate> coordinates) => 
        _coordinates = coordinates.ToHashSet();

    public static FleetShip Create(Coordinate coordinate, params Coordinate[] coordinates)
    {
        var allCoordinates = new[] { coordinate }.Concat(coordinates).ToHashSet();

        if (!AreCoordinatesConnected(allCoordinates))
        {
            throw new FleetShipCoordinatesAreDisconnected();
        }
        
        return new FleetShip(allCoordinates);
    }

    public ShootResult ReceiveShot(Coordinate coordinate) => 
        _coordinates.Contains(coordinate) ? ShootResult.Hit : ShootResult.Miss;

    public bool IsOverlappingWith(FleetShip another) => 
        _coordinates.Overlaps(another._coordinates);

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