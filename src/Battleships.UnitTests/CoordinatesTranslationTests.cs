using Battleships.Console;
using Battleships.Console.Application;
using Battleships.Console.Application.Fleets;
using FluentAssertions;

namespace Battleships.UnitTests;

public class CoordinatesTranslationTests
{
    [Fact]
    public void translates_fleet_coordinates_to_grid_one()
    {
        GridCoordinates.From(new Coordinates(4, 5))
            .Should().BeEquivalentTo(GridCoordinates.From("F5"));
    }
    
    [Fact]
    public void translates_grid_coords_to_fleet_one()
    {
        GridCoordinates.From("D7").ToFleetCoords()
            .Should().Be(new Coordinates(6, 3));
    }
}