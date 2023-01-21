using Battleships.Console.Fleets;
using FluentAssertions;

namespace Battleships.UnitTests.Fleets;

public class CoordinatesSetsEqualityTests
{
    [Fact]
    public void single_element_set()
    {
        (CoordinatesSet.Create((0, 0)) == CoordinatesSet.Create((0,0))).Should().BeTrue();
        
        (CoordinatesSet.Create((0, 0)) == CoordinatesSet.Create((1,0))).Should().BeFalse();
    }
    
    [Fact]
    public void two_element_set()
    {
        (CoordinatesSet.Create((0, 1),(1,0)) == CoordinatesSet.Create((1,0),(0,1))).Should().BeTrue();
        
        (CoordinatesSet.Create((0, 1),(1,0)) == CoordinatesSet.Create((0,1),(1,0))).Should().BeTrue();
        
        (CoordinatesSet.Create((0, 1),(0,1)) == CoordinatesSet.Create((0,1),(0,1))).Should().BeTrue();
        
        (CoordinatesSet.Create((0, 1),(1,1)) == CoordinatesSet.Create((0,1),(0,1))).Should().BeFalse();
    }
    
    [Fact]
    public void many_element_set_with_same_elements()
    {
        (CoordinatesSet.Create((0, 1),(0,1), (0,2), (0,3),(0,3), (3,0), (3,0), (4,1),(4,2)) == 
         CoordinatesSet.Create((0, 1),(4,1), (0,1), (0,3),(4,2), (0,3), (3,0), (0,2),(3,0))).Should().BeTrue();
    }

    [Fact]
    public void coordinates_with_same_values_on_different_position()
    {
        (CoordinatesSet.Create((0, 3)) == CoordinatesSet.Create((3, 0))).Should().BeFalse();
    }
}