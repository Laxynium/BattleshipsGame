using FluentAssertions;
using static Battleships.UnitTests.Builders.CoordinatesSetBuilder;

namespace Battleships.UnitTests.Fleets;

public class CoordinatesConnectionTests
{
    [Fact]
    public void for_correctly_connected_coordinates_returns_true()
    {
        CreateCoordinatesSet((0, 0), (0, 1))
            .AreCoordinatesConnect()
            .Should().BeTrue();
        
        CreateCoordinatesSet((0, 0), (0, -1))
            .AreCoordinatesConnect()
            .Should().BeTrue();
        
        CreateCoordinatesSet((5, 6), (6, 6), (4,6), (4,7))
            .AreCoordinatesConnect()
            .Should().BeTrue();
    }
    
    [Fact]
    public void for_two_disconnected_coordinates_returns_false()
    {
        CreateCoordinatesSet((0, 0), (0, 2))
            .AreCoordinatesConnect()
            .Should().BeFalse();
    }

    [Fact]
    public void for_coordinates_connected_diagonally_returns_false()
    {
        CreateCoordinatesSet((5, 5), (6, 6))
            .AreCoordinatesConnect()
            .Should().BeFalse();
        
        CreateCoordinatesSet((5, 5), (4,4), (6,6))
            .AreCoordinatesConnect()
            .Should().BeFalse();
        
        CreateCoordinatesSet((5, 5), (4,6), (6,4))
            .AreCoordinatesConnect()
            .Should().BeFalse();
    }
    
    [Fact]
    public void for_two_islands_of_disconnected_coordinates_returns_false()
    {
        CreateCoordinatesSet(
                (2, 2), (2, 3), (2, 4),
                (5, 5), (4, 5))
            .AreCoordinatesConnect()
            .Should().BeFalse();
    }
}