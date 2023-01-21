using Battleships.Console.Fleets;
using FluentAssertions;

namespace Battleships.UnitTests.Fleets;

public class CoordinatesConnectionTests
{
    [Fact]
    public void for_correctly_connected_coordinates_returns_true()
    {
        CoordinatesSet.Create((0, 0), (0, 1))
            .AreCoordinatesConnect()
            .Should().BeTrue();
        
        CoordinatesSet.Create((0, 0), (0, -1))
            .AreCoordinatesConnect()
            .Should().BeTrue();
        
        CoordinatesSet.Create((5, 6), (6, 6), (4,6), (4,7))
            .AreCoordinatesConnect()
            .Should().BeTrue();
    }
    
    [Fact]
    public void for_two_disconnected_coordinates_returns_false()
    {
        CoordinatesSet.Create((0, 0), (0, 2))
            .AreCoordinatesConnect()
            .Should().BeFalse();
    }

    [Fact]
    public void for_coordinates_connected_diagonally_returns_false()
    {
        CoordinatesSet.Create((5, 5), (6, 6))
            .AreCoordinatesConnect()
            .Should().BeFalse();
        
        CoordinatesSet.Create((5, 5), (4,4), (6,6))
            .AreCoordinatesConnect()
            .Should().BeFalse();
        
        CoordinatesSet.Create((5, 5), (4,6), (6,4))
            .AreCoordinatesConnect()
            .Should().BeFalse();
    }
    
    [Fact]
    public void for_two_islands_of_disconnected_coordinates_returns_false()
    {
        CoordinatesSet.Create(
                (2, 2), (2, 3), (2, 4),
                (5, 5), (4, 5))
            .AreCoordinatesConnect()
            .Should().BeFalse();
    }
}