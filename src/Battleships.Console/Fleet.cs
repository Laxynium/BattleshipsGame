namespace Battleships.Console;

public record Coordinate(int X, int Y)
{
    public static implicit operator Coordinate((int x, int y) coordinate) =>
        new(coordinate.x, coordinate.y);

    public IEnumerable<Coordinate> GetNeighbours()
    {
        yield return this with { X = X - 1 };
        yield return this with { X = X + 1 };
        yield return this with { Y = Y - 1 };
        yield return this with { Y = Y + 1 };
    }
}

public record FleetShip
{
    private readonly HashSet<Coordinate> _coordinates;

    private FleetShip(IEnumerable<Coordinate> coordinates) => 
        _coordinates = coordinates.ToHashSet();

    public static FleetShip Create(Coordinate coordinate, params Coordinate[] coordinates)
    {
        var allCoordinates = new[] { coordinate }.Concat(coordinates).ToList();

        if (!AreCoordinatesConnected(allCoordinates))
        {
            throw new FleetShipCoordinatesAreDisconnected();
        }
        
        return new(allCoordinates);
    }

    private static bool AreCoordinatesConnected(IReadOnlyList<Coordinate> allCoordinates)
    {
        if (allCoordinates.Count == 1)
        {
            return true;
        }

        var segment = new List<Coordinate>(){allCoordinates[0]};
        var queue = new Queue<Coordinate>(segment);
        while (queue.Count > 0)
        {
            var coordinate = queue.Dequeue();

            var nextCoordinatesToVisit = coordinate
                .GetNeighbours()
                .Intersect(allCoordinates)
                .Except(segment)
                .ToList();
            
            segment.AddRange(nextCoordinatesToVisit);
            foreach (var nextCoordinate in nextCoordinatesToVisit)
            {
                queue.Enqueue(nextCoordinate);
            }
        }

        return segment.Count == allCoordinates.Count;
    }

    public Fleet.ShootResult ReceiveShot(Coordinate coordinate) => 
        _coordinates.Contains(coordinate) ? Fleet.ShootResult.Hit : Fleet.ShootResult.Miss;

    public bool IsOverlappingWith(FleetShip another) => 
        _coordinates.Overlaps(another._coordinates);
}

public record Fleet
{
    private readonly List<FleetShip> _ships;

    private Fleet(IEnumerable<FleetShip> ships) => 
        _ships = ships.ToList();

    public static Fleet Create(FleetShip ship, params FleetShip[] ships)
    {
        var allShips = new[] { ship }.Concat(ships).ToList();

        var areThereOverlappingShips = CartesianProduct(allShips)
            .Where(c => c.x != c.y)
            .Any(c => c.x.IsOverlappingWith(c.y));

        if (areThereOverlappingShips)
        {
            throw new ShipsAreOverlappingException();
        }
        
        return new(allShips);
    }

    public ShootResult ReceiveShot(Coordinate coordinate)
    {
        foreach (var ship in _ships)
        {
            var result = ship.ReceiveShot(coordinate);
            if (result == ShootResult.Hit)
            {
                return ShootResult.Hit;
            }
        }

        return ShootResult.Miss;
    } 
        

    public enum ShootResult
    {
        Hit, Miss
    }

    private static IEnumerable<(T x, T y)> CartesianProduct<T>(IList<T> items) => 
        from x in items 
        from y in items 
        select (x, y);
}

public class ShipsAreOverlappingException : Exception
{
}
public class FleetShipCoordinatesAreDisconnected : Exception
{
}
