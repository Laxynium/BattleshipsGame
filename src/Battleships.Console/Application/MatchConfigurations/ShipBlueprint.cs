using Battleships.Console.Application.Fleets;
using CSharpFunctionalExtensions;

namespace Battleships.Console.Application.MatchConfigurations;

public record ShipBlueprintName(string Name)
{
    public static implicit operator ShipBlueprintName(string name) => new(name);
}
public class ShipBlueprint : ValueObject
{
    public ShipBlueprintName ShipBlueprintName { get; }
    public CoordinatesSet Set { get; }

    private ShipBlueprint(ShipBlueprintName shipBlueprintName, CoordinatesSet coordinatesSet)
    {
        ShipBlueprintName = shipBlueprintName;
        Set = coordinatesSet;
    }

    public static ShipBlueprint FromText(ShipBlueprintName name, string text)
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

        return Create(name, CoordinatesSet.Create(coords.First(), coords.Skip(1).ToArray()));
    }

    public static ShipBlueprint Create(ShipBlueprintName name, CoordinatesSet coordinatesSet)
    {
        if (!coordinatesSet.Set.Contains((0, 0)))
        {
            throw new Exception("Ship blueprint coords need to be expressed relatively to (0,0) origin");
        }

        if (coordinatesSet.Set.Any(x => x.X < 0 || x.Y < 0))
        {
            throw new Exception("Ship blueprint coords need to >= 0");
        }
        
        return new ShipBlueprint(name, coordinatesSet);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Set;
    }
}