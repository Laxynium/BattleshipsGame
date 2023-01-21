namespace Battleships.Console.Fleets;

public record Coordinates(int X, int Y)
{
    public static implicit operator Coordinates((int x, int y) coordinate) =>
        new(coordinate.x, coordinate.y);

    public IEnumerable<Coordinates> GetNeighbourhood()
    {
        yield return this with { X = X - 1 };
        yield return this with { X = X + 1 };
        yield return this with { Y = Y - 1 };
        yield return this with { Y = Y + 1 };
    }
}

public class CoordinatesSet
{
    private readonly HashSet<Coordinates> _set;

    public IReadOnlySet<Coordinates> Set => _set.ToHashSet();

    private CoordinatesSet(IEnumerable<Coordinates> set)
    {
        _set = set.ToHashSet();
    }

    public static CoordinatesSet Create(Coordinates head, params Coordinates[] tail) => new(new[] { head }.Concat(tail));

    public bool AreCoordinatesConnect()
    {
        var connectedCoordinatesSets = new List<Coordinates> {_set.First()};
        var queue = new Queue<Coordinates>(connectedCoordinatesSets);
        while (queue.Count > 0)
        {
            var coordinate = queue.Dequeue();

            var nextCoordinatesToVisit = coordinate
                .GetNeighbourhood()
                .Intersect(_set)
                .Except(connectedCoordinatesSets)
                .ToList();
            
            connectedCoordinatesSets.AddRange(nextCoordinatesToVisit);
            
            foreach (var nextCoordinate in nextCoordinatesToVisit)
            {
                queue.Enqueue(nextCoordinate);
            }
        }

        return connectedCoordinatesSets.Count == _set.Count;
    }
}