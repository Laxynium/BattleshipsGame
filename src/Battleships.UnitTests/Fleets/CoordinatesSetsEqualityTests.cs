using FluentAssertions;
using static Battleships.UnitTests.Builders.CoordinatesSetBuilder;

namespace Battleships.UnitTests.Fleets;

public class CoordinatesSetsEqualityTests
{
    [Fact]
    public void single_element_set()
    {
        (CreateCoordinatesSet((0, 0)) == CreateCoordinatesSet((0,0))).Should().BeTrue();
        
        (CreateCoordinatesSet((0, 0)) == CreateCoordinatesSet((1,0))).Should().BeFalse();
    }
    
    [Fact]
    public void two_element_set()
    {
        (CreateCoordinatesSet((0, 1),(1,0)) == CreateCoordinatesSet((1,0),(0,1))).Should().BeTrue();
        
        (CreateCoordinatesSet((0, 1),(1,0)) == CreateCoordinatesSet((0,1),(1,0))).Should().BeTrue();
        
        (CreateCoordinatesSet((0, 1),(0,1)) == CreateCoordinatesSet((0,1),(0,1))).Should().BeTrue();
        
        (CreateCoordinatesSet((0, 1),(1,1)) == CreateCoordinatesSet((0,1),(0,1))).Should().BeFalse();
    }
    
    [Fact]
    public void many_element_set_with_same_elements()
    {
        (CreateCoordinatesSet((0, 1),(0,1), (0,2), (0,3),(0,3), (3,0), (3,0), (4,1),(4,2)) == 
         CreateCoordinatesSet((0, 1),(4,1), (0,1), (0,3),(4,2), (0,3), (3,0), (0,2),(3,0))).Should().BeTrue();
    }

    [Fact]
    public void coordinates_with_same_values_on_different_position()
    {
        (CreateCoordinatesSet((0, 3)) == CreateCoordinatesSet((3, 0))).Should().BeFalse();
    }
}