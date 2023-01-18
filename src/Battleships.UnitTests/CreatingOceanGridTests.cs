using Battleships.Console;
using FluentAssertions;

namespace Battleships.UnitTests;

public class CreatingOceanGridTests
{
    [Fact]
    public void can_be_created_with_one_ship()
    {
        var result = OceanGrid.Create(4, 4, new Ship());
        
        result.IsSuccess.Should().BeTrue();
    }
    
    public void cannot_have_negative_or_0_size(){}
    
    public void cannot_be_created_without_ships(){}
    
    public void cannot_be_created_when_there_are_ships_collisions(){}
    
    public void cannot_be_created_when_there_are_ships_outside_boundaries(){}
}
