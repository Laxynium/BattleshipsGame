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
        var oceanGrid = result.Value;
        oceanGrid.Width.Should().Be(4);
        oceanGrid.Height.Should().Be(4);
    }

    [Fact]
    public void can_be_created_with_two_ships()
    {
        var result = OceanGrid.Create(4, 4, new Ship(), new Ship());
        
        result.IsSuccess.Should().BeTrue();
        var oceanGrid = result.Value;
        oceanGrid.Width.Should().Be(4);
        oceanGrid.Height.Should().Be(4);
    }
    
    [Fact]
    public void can_be_created_with_many_ships()
    {
        var result = OceanGrid.Create(4, 4, new Ship(), new Ship(), new Ship(), new Ship(), new Ship());
        
        result.IsSuccess.Should().BeTrue();
        var oceanGrid = result.Value;
        oceanGrid.Width.Should().Be(4);
        oceanGrid.Height.Should().Be(4);
    }
    
    public void cannot_have_negative_or_0_size(){}
    
    public void cannot_be_created_without_ships(){}
    
    public void cannot_be_created_when_there_are_ships_collisions(){}
    
    public void cannot_be_created_when_there_are_ships_outside_boundaries(){}
}
