using CSharpFunctionalExtensions;

namespace Battleships.Console.Fleets;

public class Coordinates : ValueObject
{
    public int X { get; }
    public int Y { get; }
    
    public Coordinates(int x, int y)
    {
        Y = y;
        X = x;
    }

    public static implicit operator Coordinates((int x, int y) coordinate) =>
        new(coordinate.x, coordinate.y);

    public IEnumerable<Coordinates> GetNeighbourhood()
    {
        yield return new Coordinates(X - 1, Y);
        yield return new Coordinates(X + 1, Y);
        yield return new Coordinates(X, Y - 1);
        yield return new Coordinates(X, Y + 1);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return X;
        yield return Y;
    }
}

public sealed class CoordinatesSet : ValueObject
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

    public static bool AreSomeOverlapping(params CoordinatesSet[] coordinatesSets) =>
        CartesianProduct(coordinatesSets)
            .Where(c => c.x != c.y)
            .Any(c => c.x.IsOverlappingWith(c.y));

    private bool IsOverlappingWith(CoordinatesSet another) => 
        _set.Overlaps(another._set);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        var sorted = new SortedSet<Coordinates>(_set);
        return sorted;
    }

    private static IEnumerable<(T x, T y)> CartesianProduct<T>(IList<T> items) => 
        from x in items 
        from y in items 
        select (x, y);
}