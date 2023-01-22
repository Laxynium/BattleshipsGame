using Battleships.Console.Fleets;
using static Battleships.UnitTests.Builders.CoordinatesSetBuilder;

namespace Battleships.UnitTests.Builders;

public static class FleetShipBuilder
{
    public static FleetShip CreateShip(FleetShipId id, Coordinates head, params Coordinates[] tail) => 
        FleetShip.Create(id, CreateCoordinatesSet(head, tail));
}