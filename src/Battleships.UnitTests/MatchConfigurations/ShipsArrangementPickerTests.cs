using Battleships.Console.Application.Fleets;
using Battleships.Console.Application.MatchConfigurations;
using FluentAssertions;

namespace Battleships.UnitTests.MatchConfigurations;

public class ShipsArrangementPickerTests
{
    [Fact]
    public void for_generated_ships_arrangements_picks_arrangement_consisting_of_valid_numbers_of_ships_on_2x4_grid()
    {
        var ships = new List<CoordinatesSet>
        {
            ShipBlueprint.FromText("a", "--").Set,
            ShipBlueprint.FromText("a", "---").Set
        }.ToArray();
        var constrains = new GridConstrains(2, 4);
        var possibleShipsArrangements = GeneratedShipsArrangements(ships, constrains);

        var result =
            ShipsArrangementPicker.PickShipsArrangement(ships, AlwaysTakeFirstOne, i => possibleShipsArrangements[i]);

        result.Should().HaveCount(2);
        result.Should().Contain(x => x.Set.Count == 2);
        result.Should().Contain(x => x.Set.Count == 3);
    }

    [Fact]
    public void for_generated_ships_arrangements_picks_arrangement_consisting_of_valid_numbers_of_ships_on_1x4_grid()
    {
        var ships = new List<CoordinatesSet>
        {
            ShipBlueprint.FromText("a", "--").Set,
            ShipBlueprint.FromText("a", "---").Set
        }.ToArray();

        var constrains = new GridConstrains(1, 4);
        var possibleShipsArrangements = GeneratedShipsArrangements(ships, constrains);

        var result =
            ShipsArrangementPicker.PickShipsArrangement(ships, AlwaysTakeFirstOne, i => possibleShipsArrangements[i]);

        result.Should().HaveCount(0);
    }

    [Fact]
    public void when_there_are_many_possible_arrangements_to_choose_from()
    {
        var ships = new List<CoordinatesSet>
        {
            ShipBlueprint.FromText("a", "--").Set,
            ShipBlueprint.FromText("a", "--").Set
        }.ToArray();
        var constrains = new GridConstrains(10, 10);
        var possibleShipsArrangements = GeneratedShipsArrangements(ships, constrains);

        var result =
            ShipsArrangementPicker.PickShipsArrangement(ships, AlwaysTakeFirstOne, i => possibleShipsArrangements[i]);

        result.Should().HaveCount(2);
    }

    [Fact]
    public void when_there_are_many_not_overlapping_choices()
    {
        var ships = new List<CoordinatesSet>
        {
            ShipBlueprint.FromText("a", "---").Set,
            ShipBlueprint.FromText("a", "--").Set
        }.ToArray();
        var constrains = new GridConstrains(2, 3);
        var possibleShipsArrangements = new[]
        {
            new[]
            {
                CoordinatesSet.Create((0, 0), (0, 1), (0, 2)),
                CoordinatesSet.Create((1, 0), (1, 1), (1, 2))
            },
            new[]
            {
                CoordinatesSet.Create((0, 0), (1, 0)),
                CoordinatesSet.Create((0, 1), (1, 1)),
                CoordinatesSet.Create((0, 2), (1, 2)),

                CoordinatesSet.Create((0, 0), (0, 1)),
                CoordinatesSet.Create((0, 1), (0, 2)),

                CoordinatesSet.Create((1, 0), (1, 1)),
                CoordinatesSet.Create((1, 1), (1, 2))
            }
        };
        var picks = new[] { new[]{0, 1}, new []{ 0, 1}, new []{0, 1}, new []{ 0, 3} };

        var picker = CreatePickerFrom(picks);

        var result = ShipsArrangementPicker.PickShipsArrangement(ships,
            picker,
            i => possibleShipsArrangements[i]);

        result.Should().ContainInOrder(new[]
        {
            CoordinatesSet.Create((0, 0), (0, 1), (0, 2)),
            CoordinatesSet.Create((1, 1), (1, 2))
        });
    }

    private static ShipsArrangementPicker.ShipArrangementPicker CreatePickerFrom(int[][] picks)
    {
        var firstIndex = 0;
        var secondIndex = 0;
        return (shipArrangements) =>
        {
            var result = shipArrangements[picks[firstIndex][secondIndex]];
            if (secondIndex >= picks[secondIndex].Length-1)
            {
                firstIndex++;
                secondIndex = 0;
            }
            else
            {
                secondIndex++;
            }
            
            return result;
        };
    }

    private static IReadOnlyCollection<CoordinatesSet>[] GeneratedShipsArrangements(CoordinatesSet[] ships,
        GridConstrains constrains) =>
        ships
            .Select(x => ArrangementsGenerator.GenerateFor(x, constrains))
            .ToArray();


    private static CoordinatesSet AlwaysTakeFirstOne(IReadOnlyList<CoordinatesSet> actualShipArrangements) =>
        actualShipArrangements.First();
}