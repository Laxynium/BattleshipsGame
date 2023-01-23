using Battleships.Console.Application.Fleets;
using FluentAssertions;
using static Battleships.UnitTests.Builders.FleetShipBuilder;

namespace Battleships.UnitTests.Fleets;

public class CreatingFleetShipTests
{
    [Fact]
    public void can_create_ship()
    {
        CreateShip("1",(0, 0));
        
        CreateShip("1", (0, 0), (0, 1));
        
        CreateShip("1", (0, 0), (0, 1), (0, 2));
        
        CreateShip("1", (1, 0), (3, 0), (2, 0));
        
        CreateShip("1", (5, 5), (5, 4), (6, 4), (6,3));
        
        CreateShip("1", (6,3), (5, 4), (6, 4), (5, 5));
    }

    [Fact]
    public void ships_coordinates_can_be_duplicated()
    {
        CreateShip("1", (1, 0), (1, 0));
        
        CreateShip("1", (1, 0), (1, 0), (2, 0), (1, 0));
        
        CreateShip("1", (1, 3), (1, 3), (2, 3), (1, 3), (2,3));
    }
    
    [Fact]
    public void cannot_create_ship_when_its_coordinates_are_disconnected()
    {
        var action = () => CreateShip("1", (3, 2), (0,2));
        action.Should().Throw<CoordinatesAreDisconnectedException>();
    }
}