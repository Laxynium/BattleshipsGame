namespace Battleships.Console.Application.Fleets;

public record FleetShipId(string Value)
{
    public static implicit operator FleetShipId(string value) => new(value);
}