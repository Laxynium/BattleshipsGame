using Battleships.Console.Fleets;
using static Battleships.UnitTests.Builders.CoordinatesSetBuilder;

namespace Battleships.UnitTests.Builders;

public static class FleetShipBuilder
{
    public static FleetShip CreateShip(Coordinates head, params Coordinates[] tail) => 
        FleetShip.Create(CreateCoordinatesSet(head, tail));
}