using Battleships.Console.Fleets;
using FluentAssertions;
using static Battleships.UnitTests.Builders.FleetShipBuilder;

namespace Battleships.UnitTests.Fleets;

public class CreatingFleetShipTests
{
    [Fact]
    public void can_create_ship()
    {
        CreateShip((0, 0));
        
        CreateShip((0, 0), (0, 1));
        
        CreateShip((0, 0), (0, 1), (0, 2));
        
        CreateShip((1, 0), (3, 0), (2, 0));
        
        CreateShip((5, 5), (5, 4), (6, 4), (6,3));
        
        CreateShip((6,3), (5, 4), (6, 4), (5, 5));
    }

    [Fact]
    public void ships_coordinates_can_be_duplicated()
    {
        CreateShip((1, 0), (1, 0));
        
        CreateShip((1, 0), (1, 0), (2, 0), (1, 0));
        
        CreateShip((1, 3), (1, 3), (2, 3), (1, 3), (2,3));
    }
    
    [Fact]
    public void cannot_create_ship_when_its_coordinates_are_disconnected()
    {
        var action = () => CreateShip((0, 0), (0,2));
        action.Should().Throw<FleetShipCoordinatesAreDisconnectedException>();
    }

    [Fact]
    public void cannot_create_ship_when_its_coordinates_are_connected_only_diagonally()
    {
        var action = () => CreateShip((5, 5), (6,6));
        action.Should().Throw<FleetShipCoordinatesAreDisconnectedException>();
        
        action = () => CreateShip((5, 5), (4,4), (6,6));
        action.Should().Throw<FleetShipCoordinatesAreDisconnectedException>();
        
        action = () => CreateShip((5, 5), (4,6), (6,4));
        action.Should().Throw<FleetShipCoordinatesAreDisconnectedException>();
    }
    
    [Fact]
    public void cannot_create_ship_when_there_two_islands_of_disconnected_coordinates()
    {
        var action = () => CreateShip(
            (2, 2), (2, 3), (2, 4),
            (5, 5), (4, 5));
        action.Should().Throw<FleetShipCoordinatesAreDisconnectedException>();
    }
}