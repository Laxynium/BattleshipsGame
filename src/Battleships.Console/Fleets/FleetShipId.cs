namespace Battleships.Console.Fleets;

public record FleetShipId(string Value)
{
    public static implicit operator FleetShipId(string value) => new(value);
}