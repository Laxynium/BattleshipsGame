using Battleships.Console.Application.MatchConfigurations;
using FluentAssertions;
using static Battleships.UnitTests.Builders.CoordinatesSetBuilder;
namespace Battleships.UnitTests.MatchConfigurations;

public class ShipBlueprintTests
{
    [Fact]
    public void error_when_there_are_negative_coordinates()
    {
        var action = () => ShipBlueprint.Create(CreateCoordinatesSet((-1, 0), (0, 0)));
        action.Should().Throw<Exception>().WithMessage("*>= 0*");
    }
    
    [Fact]
    public void error_when_coords_are_not_relative_to_0_0_origin()
    {
        var action = () => ShipBlueprint.Create(CreateCoordinatesSet((1, 1), (2, 1)));
        action.Should().Throw<Exception>().WithMessage("*relative*origin*");
    }

    [Fact]
    public void can_be_created()
    {
        var action = () => ShipBlueprint.Create(CreateCoordinatesSet((0, 1), (0, 0)));
        action.Should().NotThrow();
    }
}