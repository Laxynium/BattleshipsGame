using Battleships.Console.Fleets;
using FluentAssertions;
using static Battleships.UnitTests.Builders.FleetShipBuilder;

namespace Battleships.UnitTests.Fleets;

public class CreatingFleetTests
{
    [Fact]
    public void can_create_when_there_are_no_overlaps()
    {
        Fleet.Create(
            CreateShip("1", (0, 0), (0, 1), (0, 2)),
            CreateShip("2", (1, 0), (1, 1), (1, 2)));
    }

    [Fact]
    public void ships_in_fleet_cannot_be_overlapping()
    {
        var action = () => Fleet.Create(
            CreateShip("1", (0, 0), (0, 1), (0, 2)),
            CreateShip("2", (0, 1), (1, 1), (2, 1)));

        action.Should().Throw<ShipsAreOverlappingException>();
    }
}