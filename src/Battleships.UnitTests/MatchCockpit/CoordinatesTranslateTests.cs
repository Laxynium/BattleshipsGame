using Battleships.Console.Fleets;
using Battleships.Console.MatchCockpit;
using FluentAssertions;

namespace Battleships.UnitTests.MatchCockpit;

public class CoordinatesTranslateTests
{
    [Fact]
    public void translates_fleet_coordinates_to_grid_one()
    {
        var fleetCoords = new Coordinates(4, 5);

        var gridCoords = CoordinatesTranslator.AGridCoordinates(fleetCoords);

        gridCoords.Should().BeEquivalentTo("F5");
    }
    
    [Fact]
    public void translates_grid_coords_to_fleet_one()
    {
        var gridCords = "D7";

        var fleetCords = CoordinatesTranslator.AFleetCoordinates(gridCords);

        fleetCords.Should().Be(new Coordinates(6, 3));
    }
}