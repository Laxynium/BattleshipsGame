using Battleships.Console.Fleets;
using FluentAssertions;

namespace Battleships.UnitTests.Fleets;

public class CreatingFleetTests
{
    [Fact]
    public void can_create_when_there_are_no_overlaps()
    {
        Fleet.Create(
            FleetShip.Create((0, 0), (0, 1), (0, 2)),
            FleetShip.Create((1, 0), (1, 1), (1, 2)));
    }

    [Fact]
    public void ships_in_fleet_cannot_be_overlapping()
    {
        var action = () => Fleet.Create(
            FleetShip.Create((0, 0), (0, 1), (0, 2)),
            FleetShip.Create((0, 1), (1, 1), (2, 1)));

        action.Should().Throw<ShipsAreOverlappingException>();
    }
}