using Battleships.Console.Fleets;

namespace Battleships.UnitTests.Builders;

public static class FleetShipBuilder
{
    public static FleetShip CreateShip(Coordinates head, params Coordinates[] tail) => 
        FleetShip.Create(head, tail);
}