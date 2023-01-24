using Battleships.Console.Application.Fleets;
using Battleships.Console.Application.MatchConfigurations;
using FluentAssertions;

namespace Battleships.UnitTests.MatchConfigurations;

public class SingleShipBlueprintArrangementsGenerationTests
{
    [Fact]
    public void generate_using_only_translations_for_ship_of_size_1_on_3x3_grid()
    {
        var shipBlueprint = ShipBlueprint.FromText("a", "-");

        var result = ArrangementsGenerator.GenerateUsingTranslationsFor(shipBlueprint.Set,
            new GridConstrains(3, 3));

        result.Should().HaveCount(9);
        result.Should().Contain(new[]
        {
            CoordinatesSet.Create((0, 0)),
            CoordinatesSet.Create((0, 1)),
            CoordinatesSet.Create((0, 2)),
            CoordinatesSet.Create((1, 0)),
            CoordinatesSet.Create((1, 1)),
            CoordinatesSet.Create((1, 2)),
            CoordinatesSet.Create((2, 0)),
            CoordinatesSet.Create((2, 1)),
            CoordinatesSet.Create((2, 2))
        });
    }

    [Fact]
    public void generate_using_only_translations_for_ship_of_size_2_on_3x3_grid()
    {
        var shipBlueprint = ShipBlueprint.FromText("a", "--");

        var result = ArrangementsGenerator.GenerateUsingTranslationsFor(shipBlueprint.Set,
            new GridConstrains(3, 3));

        result.Should().HaveCount(6);
        result.Should().Contain(new[]
        {
            CoordinatesSet.Create((0, 0), (1, 0)),
            CoordinatesSet.Create((1, 0), (2, 0)),

            CoordinatesSet.Create((0, 1), (1, 1)),
            CoordinatesSet.Create((1, 1), (2, 1)),

            CoordinatesSet.Create((0, 2), (1, 2)),
            CoordinatesSet.Create((1, 2), (2, 2)),
        });
    }

    [Fact]
    public void does_not_generate_arrangements_outside_of_grid_boundaries_ship_of_size_4_on_3x3_grid()
    {
        var shipBlueprint = ShipBlueprint.FromText("a", "----");

        var result = ArrangementsGenerator.GenerateUsingTranslationsFor(shipBlueprint.Set,
            new GridConstrains(3, 3));

        result.Should().HaveCount(0);
    }

    [Fact]
    public void does_not_generate_arrangements_outside_of_grid_boundaries_ship_of_size_4_on_4x3_grid()
    {
        var shipBlueprint = ShipBlueprint.FromText("a", "----");

        var result = ArrangementsGenerator.GenerateUsingTranslationsFor(shipBlueprint.Set,
            new GridConstrains(4, 3));

        result.Should().HaveCount(3);
        result.Should().Contain(new[]
        {
            CoordinatesSet.Create((0, 0), (1, 0), (2, 0), (3, 0)),
            CoordinatesSet.Create((0, 1), (1, 1), (2, 1), (3, 1)),
            CoordinatesSet.Create((0, 2), (1, 2), (2, 2), (3, 2))
        });
    }

    [Fact]
    public void generate_using_only_rotations_for_ship_of_size_2_on_3x3_grid()
    {
        var shipBlueprint = ShipBlueprint.FromText("a", "--");

        var result = ArrangementsGenerator.GenerateUsingRotationsFor(shipBlueprint.Set,
            new GridConstrains(3, 3));

        result.Should().HaveCount(2);
        result.Should().Contain(new[]
        {
            CoordinatesSet.Create((0, 0), (1, 0)),
            CoordinatesSet.Create((0, 0), (0, 1)),
        });
    }

    [Fact]
    public void generate_using_only_rotations_for_ship_of_size_4_on_3x3_grid()
    {
        var shipBlueprint = ShipBlueprint.FromText("a", "----");

        var result = ArrangementsGenerator.GenerateUsingRotationsFor(shipBlueprint.Set,
            new GridConstrains(3, 3));

        result.Should().HaveCount(0);
    }

    [Fact]
    public void generate_using_only_rotations_for_ship_of_size_4_on_1x4_grid()
    {
        var shipBlueprint = ShipBlueprint.FromText("a", "----");

        var result = ArrangementsGenerator.GenerateUsingRotationsFor(shipBlueprint.Set,
            new GridConstrains(1, 4));

        result.Should().HaveCount(1);
        result.Should().Contain(new[]
        {
            CoordinatesSet.Create((0, 0), (0, 1), (0, 2), (0, 3))
        });
    }

    [Fact]
    public void generate_using_both_rotations_and_translations_for_ship_of_size_2_on_3x3_grid()
    {
        var shipBlueprint = ShipBlueprint.FromText("a", "--");

        // var result = ArrangementsGenerator.GenerateUsingBothTranslationsAndRotationsFor(shipBlueprint, 
        //     new GridConstrains(3, 3));
    }
}