using Battleships.Console.Fleets;
using FluentAssertions;
using static Battleships.UnitTests.Builders.CoordinatesSetBuilder;

namespace Battleships.UnitTests.Fleets;

public class CoordinatesConnectionTests
{
    [Fact]
    public void for_correctly_connected_coordinates_returns_true()
    {
        AreCoordinatesConnected((0, 0), (0, 1))
            .Should().BeTrue();

        AreCoordinatesConnected((0, 0), (0, -1))
            .Should().BeTrue();

        AreCoordinatesConnected((5, 6), (6, 6), (4, 6), (4, 7))
            .Should().BeTrue();
    }

    [Fact]
    public void for_two_disconnected_coordinates_returns_false()
    {
        AreCoordinatesConnected((0, 0), (0, 2))
            .Should().BeFalse();
    }

    [Fact]
    public void for_coordinates_connected_diagonally_returns_false()
    {
        AreCoordinatesConnected((5, 5), (6, 6))
            .Should().BeFalse();

        AreCoordinatesConnected((5, 5), (4, 4), (6, 6))
            .Should().BeFalse();

        AreCoordinatesConnected((5, 5), (4, 6), (6, 4))
            .Should().BeFalse();
    }

    [Fact]
    public void for_two_islands_of_disconnected_coordinates_returns_false()
    {
        AreCoordinatesConnected((2, 2), (2, 3), (2, 4), (5, 5), (4, 5))
            .Should().BeFalse();
    }

    private static bool AreCoordinatesConnected(Coordinates head, params Coordinates[] tail) =>
        CreateCoordinatesSet(head, tail)
            .AreCoordinatesConnect();
}