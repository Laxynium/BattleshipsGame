using Battleships.Console.Fleets;
using FluentAssertions;
using static Battleships.UnitTests.Builders.FleetShipBuilder;

namespace Battleships.UnitTests.Fleets;

public class ShootingFleetTests
{
    [Fact]
    public void hitting_ship_with_one_coordinate_results_in_sunk()
    {
        Fleet.Create(CreateShip("1",(3, 3)), CreateShip("2",(1,1)))
            .ReceiveShot((3, 3)).Should().Be(ShotResult.ASunk(new FleetShipId("1")));

        Fleet.Create(CreateShip("1",(4, 4)), CreateShip("2",(1,1)))
            .ReceiveShot((4, 4)).Should().Be(ShotResult.ASunk(new FleetShipId("1")));

        Fleet.Create(CreateShip("1",(5, 7)), CreateShip("2",(1,1)))
            .ReceiveShot((1, 1)).Should().Be(ShotResult.ASunk(new FleetShipId("2")));
    }

    [Fact]
    public void missing_the_shot_on_ship_with_one_coordinate_results_in_miss()
    {
        var fleet = Fleet.Create(CreateShip("1",(3, 3)));

        fleet.ReceiveShot((3, 4)).Should().Be(ShotResult.AMiss());
        fleet.ReceiveShot((4, 3)).Should().Be(ShotResult.AMiss());
        fleet.ReceiveShot((4, 4)).Should().Be(ShotResult.AMiss());
    }

    [Fact]
    public void shooting_ship_with_two_coordinates()
    {
        var fleet = Fleet.Create(CreateShip("1",(3, 3), (3, 4)), CreateShip("2",(1,1)));

        fleet.ReceiveShot((3, 3)).Should().Be(ShotResult.AHit(new FleetShipId("1")));
        fleet.ReceiveShot((3, 4)).Should().Be(ShotResult.ASunk(new FleetShipId("1")));

        fleet.ReceiveShot((3, 2)).Should().Be(ShotResult.AMiss());
        fleet.ReceiveShot((3, 5)).Should().Be(ShotResult.AMiss());
        fleet.ReceiveShot((2, 3)).Should().Be(ShotResult.AMiss());
        fleet.ReceiveShot((4, 3)).Should().Be(ShotResult.AMiss());
    }


    [Fact]
    public void shooting_ship_with_many_coordinates()
    {
        var fleet = Fleet.Create(CreateShip("1", (3, 3), (3, 4), (3, 5), (3, 6), (3, 7)));

        fleet.ReceiveShot((3, 4)).Should().Be(ShotResult.AHit(new FleetShipId("1")));
        fleet.ReceiveShot((3, 5)).Should().Be(ShotResult.AHit(new FleetShipId("1")));
        fleet.ReceiveShot((3, 6)).Should().Be(ShotResult.AHit(new FleetShipId("1")));
        fleet.ReceiveShot((3, 7)).Should().Be(ShotResult.AHit(new FleetShipId("1")));

        fleet.ReceiveShot((3, 2)).Should().Be(ShotResult.AMiss());
        fleet.ReceiveShot((2, 4)).Should().Be(ShotResult.AMiss());
        fleet.ReceiveShot((4, 5)).Should().Be(ShotResult.AMiss());
        fleet.ReceiveShot((3, 9)).Should().Be(ShotResult.AMiss());
    }

    [Fact]
    public void shooting_when_there_are_two_ships()
    {
        var fleet = Fleet.Create(
            CreateShip("1", (5, 5), (6, 5)),
            CreateShip("2", (4, 4), (3, 4)));

        fleet.ReceiveShot((5, 5)).Should().Be(ShotResult.AHit(new FleetShipId("1")));
        fleet.ReceiveShot((3, 4)).Should().Be(ShotResult.AHit(new FleetShipId("2")));
        
        fleet.ReceiveShot((6, 6)).Should().Be(ShotResult.AMiss());
        fleet.ReceiveShot((4, 5)).Should().Be(ShotResult.AMiss());
    }

    [Fact]
    public void shooting_when_there_many_ships()
    {
        var fleet = Fleet.Create(
            CreateShip("1", (0, 0), (1, 0), (2, 0)),
            CreateShip("2",(4, 1), (4, 2), (4,3)),
            CreateShip("3",(7, 7)),
            CreateShip("4",(8, 0), (8, 1), (8, 2), (8,3)),
            CreateShip("5",(5, 2), (5, 3), (5,4)));
        
        fleet.ReceiveShot((1, 0)).Should().Be(ShotResult.AHit(new FleetShipId("1")));
        fleet.ReceiveShot((4, 3)).Should().Be(ShotResult.AHit(new FleetShipId("2")));
        fleet.ReceiveShot((8, 2)).Should().Be(ShotResult.AHit(new FleetShipId("4")));
        fleet.ReceiveShot((5, 4)).Should().Be(ShotResult.AHit(new FleetShipId("5")));
        fleet.ReceiveShot((7, 7)).Should().Be(ShotResult.ASunk(new FleetShipId("3")));
        
        fleet.ReceiveShot((3, 0)).Should().Be(ShotResult.AMiss());
        fleet.ReceiveShot((4, 0)).Should().Be(ShotResult.AMiss());
        fleet.ReceiveShot((6, 6)).Should().Be(ShotResult.AMiss());
        fleet.ReceiveShot((8, 5)).Should().Be(ShotResult.AMiss());
    }

    [Fact]
    public void for_two_coordinates_ship_hitting_all_of_the_coordinates_of_ship_results_in_sunk()
    {
        var fleet = Fleet.Create(CreateShip("1", (5, 5), (6, 5)), CreateShip("2", (1,1)));
        fleet.ReceiveShot((5, 5));

        fleet.ReceiveShot((6, 5)).Should().Be(ShotResult.ASunk(new FleetShipId("1")));
    }
    
    [Fact]
    public void for_many_coordinates_ship_hitting_all_of_the_coordinates_of_ship_results_in_sunk()
    {
        var fleet = Fleet.Create(CreateShip("1", (5, 5), (6, 5), (7, 5), (8, 5)), CreateShip("2", (1,1)));
        fleet.ReceiveShot((5, 5)).Should().Be(ShotResult.AHit(new FleetShipId("1")));
        fleet.ReceiveShot((6, 5)).Should().Be(ShotResult.AHit(new FleetShipId("1")));
        fleet.ReceiveShot((7, 5)).Should().Be(ShotResult.AHit(new FleetShipId("1")));

        fleet.ReceiveShot((8, 5)).Should().Be(ShotResult.ASunk(new FleetShipId("1")));
    }

    [Fact]
    public void for_one_ship_sinking_all_of_the_ships_results_in_fleet_sunk()
    {
        var fleet = Fleet.Create(CreateShip("1", (5, 5), (6, 5)));
        fleet.ReceiveShot((5, 5)).Should().Be(ShotResult.AHit(new FleetShipId("1")));

        fleet.ReceiveShot((6, 5)).Should().Be(ShotResult.AFleetSunk(new FleetShipId("1")));
    }
    
    [Fact]
    public void for_two_ships_sinking_all_of_the_ships_results_in_fleet_sunk()
    {
        var fleet = Fleet.Create(CreateShip("1", (5, 5), (6, 5)), 
            CreateShip("2", (2,2),(2,3),(2,4)));
        fleet.ReceiveShot((5, 5));
        fleet.ReceiveShot((6, 5));
        fleet.ReceiveShot((2, 2));
        fleet.ReceiveShot((2, 4));

        fleet.ReceiveShot((2, 3)).Should().Be(ShotResult.AFleetSunk(new FleetShipId("2")));
    }

    [Fact]
    public void hitting_ships_in_different_order_when_all_get_sunk_results_in_sunk()
    {
        var fleet = Fleet.Create(CreateShip("1", (5, 5), (6, 5), (4,5)), 
            CreateShip("2",(2,2),(2,3),(2,4)));
        fleet.ReceiveShot((2, 3));
        fleet.ReceiveShot((5, 5));
        fleet.ReceiveShot((2, 2));
        fleet.ReceiveShot((6, 5));
        fleet.ReceiveShot((2, 4));

        fleet.ReceiveShot((4, 5)).Should().Be(ShotResult.AFleetSunk(new FleetShipId("1")));
    }

    [Fact]
    public void for_many_ships_sinking_all_of_the_ships_results_in_fleet_sunk()
    {
        var fleet = Fleet.Create(CreateShip("1",(5, 5), (6, 5)), 
            CreateShip("2", (2,2),(2,3)),
            CreateShip("3", (7,7)),
            CreateShip("4", (9,9)),
            CreateShip("5", (3,3)));
        
        fleet.ReceiveShot((5, 5));
        fleet.ReceiveShot((6, 5));
        fleet.ReceiveShot((2, 2));
        fleet.ReceiveShot((2, 3));
        fleet.ReceiveShot((7, 7));
        fleet.ReceiveShot((9, 9));

        fleet.ReceiveShot((3, 3)).Should().Be(ShotResult.AFleetSunk(new FleetShipId("5")));
    }
}