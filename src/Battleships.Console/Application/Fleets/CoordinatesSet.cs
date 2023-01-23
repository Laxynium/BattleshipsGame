using CSharpFunctionalExtensions;

namespace Battleships.Console.Application.Fleets;

public sealed class CoordinatesSet : ValueObject
{
    private readonly HashSet<Coordinates> _set;

    public IReadOnlySet<Coordinates> Set => _set.ToHashSet();

    private CoordinatesSet(IEnumerable<Coordinates> set)
    {
        _set = set.ToHashSet();
    }

    public static CoordinatesSet Create(Coordinates head, params Coordinates[] tail)
    {
        var set = new[] { head }.Concat(tail).ToHashSet();
        if (!AreCoordinatesConnect(set))
        {
            throw new CoordinatesAreDisconnectedException();
        }
        return new CoordinatesSet(set);
    }


    public static bool AreSomeOverlapping(params CoordinatesSet[] coordinatesSets) =>
        CartesianProduct(coordinatesSets)
            .Where(c => c.x != c.y)
            .Any(c => c.x.IsOverlappingWith(c.y));

    private static bool AreCoordinatesConnect(IReadOnlyCollection<Coordinates> collection)
    {
        var connectedCoordinatesSets = new List<Coordinates> {collection.First()};
        var queue = new Queue<Coordinates>(connectedCoordinatesSets);
        while (queue.Count > 0)
        {
            var coordinate = queue.Dequeue();

            var nextCoordinatesToVisit = coordinate
                .GetNeighbourhood()
                .Intersect(collection)
                .Except(connectedCoordinatesSets)
                .ToList();
            
            connectedCoordinatesSets.AddRange(nextCoordinatesToVisit);
            
            foreach (var nextCoordinate in nextCoordinatesToVisit)
            {
                queue.Enqueue(nextCoordinate);
            }
        }

        return connectedCoordinatesSets.Count == collection.Count;
    }

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