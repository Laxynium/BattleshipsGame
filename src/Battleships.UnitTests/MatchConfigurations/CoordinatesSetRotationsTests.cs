using Battleships.Console.Application.Fleets;
using FluentAssertions;

namespace Battleships.UnitTests.MatchConfigurations;

public class CoordinatesSetRotationsTests
{
     [Fact]
    public void coordinates_set_90_degree_rotations()
    {
        CoordinatesSet.Create((0, 0), (1, 0), (2, 0))
            .Rotate(CounterclockwiseRotation.Rotation90)
            .Should().Be(CoordinatesSet.Create((0, 0), (0, 1), (0, 2)));
        
        CoordinatesSet.Create((0, 0), (-1, 0), (-2, 0))
            .Rotate(CounterclockwiseRotation.Rotation90)
            .Should().Be(CoordinatesSet.Create((0, 0), (0, -1), (0, -2)));
        
        CoordinatesSet.Create((0, 0), (-1, 0), (0, 1), (0, 2))
            .Rotate(CounterclockwiseRotation.Rotation90)
            .Should().Be(CoordinatesSet.Create((0, 0), (-1, 0), (-2, 0), (0,-1)));
    }
    
    [Fact]
    public void coordinates_set_180_degree_rotations()
    {
        CoordinatesSet.Create((0, 0), (1, 0), (2, 0))
            .Rotate(CounterclockwiseRotation.Rotation180)
            .Should().Be(CoordinatesSet.Create((0, 0), (-1, 0), (-2, 0)));
        
        CoordinatesSet.Create((0, 0), (-1, 0), (-2, 0))
            .Rotate(CounterclockwiseRotation.Rotation180)
            .Should().Be(CoordinatesSet.Create((0, 0), (1, 0), (2, 0)));
        
        CoordinatesSet.Create((0, 0), (-1, 0), (0, 1), (0, 2))
            .Rotate(CounterclockwiseRotation.Rotation180)
            .Should().Be(CoordinatesSet.Create((0, 0), (1, 0), (0, -1), (0, -2)));
    }
    
    [Fact]
    public void coordinates_set_270_degree_rotations()
    {
        CoordinatesSet.Create((0, 0), (1, 0), (2, 0))
            .Rotate(CounterclockwiseRotation.Rotation270)
            .Should().Be(CoordinatesSet.Create((0, 0), (0, -1), (0, -2)));
        
        CoordinatesSet.Create((0, 0), (-1, 0), (-2, 0))
            .Rotate(CounterclockwiseRotation.Rotation270)
            .Should().Be(CoordinatesSet.Create((0, 0), (0, 1), (0, 2)));
        
        CoordinatesSet.Create((0, 0), (-1, 0), (0, 1), (0, 2))
            .Rotate(CounterclockwiseRotation.Rotation270)
            .Should().Be(CoordinatesSet.Create((0, 0), (1, 0), (2, 0), (0,1)));
    }
    
    [Fact]
    public void coordinates_set_0_degree_rotations()
    {
        CoordinatesSet.Create((0, 0), (1, 0), (2, 0))
            .Rotate(CounterclockwiseRotation.Rotation0)
            .Should().Be(CoordinatesSet.Create((0, 0), (1, 0), (2, 0)));
        
        CoordinatesSet.Create((0, 0), (-1, 0), (-2, 0))
            .Rotate(CounterclockwiseRotation.Rotation0)
            .Should().Be(CoordinatesSet.Create((0, 0), (-1, 0), (-2, 0)));
        
        CoordinatesSet.Create((0, 0), (-1, 0), (0, 1), (0, 2))
            .Rotate(CounterclockwiseRotation.Rotation0)
            .Should().Be(CoordinatesSet.Create((0, 0), (-1, 0), (0, 1), (0, 2)));
    }
}