using Battleships.Console.Fleets;
using CSharpFunctionalExtensions;

namespace Battleships.Console.MatchConfigurations;

public class ShipBlueprint : ValueObject
{
    private readonly CoordinatesSet _coordinatesSet;

    private ShipBlueprint(CoordinatesSet coordinatesSet)
    {
        _coordinatesSet = coordinatesSet;
    }

    public static ShipBlueprint FromText(string text)
    {
        if (text.Length == 0)
        {
            throw new ArgumentException("Must contains at least on '-' character");
        }
        
        if (text.Any(x => x != '-'))
        {
            throw new ArgumentException("Only '-' character is allowed");
        }

        var coords = Enumerable.Range(0, text.Length)
            .Select(x => new Coordinates(x,0))
            .ToArray();

        return Create(CoordinatesSet.Create(coords.First(), coords.Skip(1).ToArray()));
    }

    public static ShipBlueprint Create(CoordinatesSet coordinatesSet)
    {
        if (!coordinatesSet.Set.Contains((0, 0)))
        {
            throw new Exception("Ship blueprint coords need to be expressed relatively to (0,0) origin");
        }

        if (coordinatesSet.Set.Any(x => x.X < 0 || x.Y < 0))
        {
            throw new Exception("Ship blueprint coords need to >= 0");
        }
        
        return new ShipBlueprint(coordinatesSet);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return _coordinatesSet;
    }
}