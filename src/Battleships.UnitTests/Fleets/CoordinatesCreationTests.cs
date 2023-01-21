using Battleships.Console.Fleets;
using FluentAssertions;

namespace Battleships.UnitTests.Fleets;

public class CoordinatesCreationTests
{
    [Fact]
    public void can_create_coordinates_set_when_all_coordinates_are_connected()
    {
        new Func<CoordinatesSet>(() => CreatedConnectedCoordinatesSet((0, 0), (0, 1)))
            .Should().NotThrow<CoordinatesAreDisconnectedException>();

        new Func<CoordinatesSet>(() => CreatedConnectedCoordinatesSet((0, 0), (0, -1)))
            .Should().NotThrow<CoordinatesAreDisconnectedException>();

        new Func<CoordinatesSet>(() => CreatedConnectedCoordinatesSet((5, 6), (6, 6), (4, 6), (4, 7)))
            .Should().NotThrow<CoordinatesAreDisconnectedException>();
    }

    [Fact]
    public void cannot_create_from_two_disconnected_coordinates()
    {
        new Func<CoordinatesSet>(() => CreatedConnectedCoordinatesSet((0, 0), (0, 2)))
            .Should().Throw<CoordinatesAreDisconnectedException>();
    }

    [Fact]
    public void cannot_created_for_only_diagonally_connected_coordinates()
    {
        new Func<CoordinatesSet>(() => CreatedConnectedCoordinatesSet((5, 5), (6, 6)))
            .Should().Throw<CoordinatesAreDisconnectedException>();

        new Func<CoordinatesSet>(() => CreatedConnectedCoordinatesSet((5, 5), (4, 4), (6, 6)))
            .Should().Throw<CoordinatesAreDisconnectedException>();

        new Func<CoordinatesSet>(() => CreatedConnectedCoordinatesSet((5, 5), (4, 6), (6, 4)))
            .Should().Throw<CoordinatesAreDisconnectedException>();
    }

    [Fact]
    public void cannot_created_for_two_disconnected_islands_of_coordinates()
    {
        new Func<CoordinatesSet>(() => CreatedConnectedCoordinatesSet((2, 2), (2, 3), (2, 4), (5, 5), (4, 5)))
            .Should().Throw<CoordinatesAreDisconnectedException>();
    }

    private static CoordinatesSet CreatedConnectedCoordinatesSet(Coordinates head, params Coordinates[] tail) => 
        CoordinatesSet.Create(head, tail);
}