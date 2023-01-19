using Battleships.Console.Fleets;
using FluentAssertions;

namespace Battleships.UnitTests;

public class ShootingFleetTests
{
    [Fact]
    public void shooting_ship_with_one_coordinate_when_is_hit()
    {
        Fleet.Create(FleetShip.Create((3, 3)))
            .ReceiveShot((3, 3)).Should().Be(ShootResult.Hit);

        Fleet.Create(FleetShip.Create((4, 4)))
            .ReceiveShot((4, 4)).Should().Be(ShootResult.Hit);

        Fleet.Create(FleetShip.Create((5, 7)))
            .ReceiveShot((5, 7)).Should().Be(ShootResult.Hit);
    }

    [Fact]
    public void shooting_ship_with_one_coordinate_when_is_miss()
    {
        var fleet = Fleet.Create(FleetShip.Create((3, 3)));

        fleet.ReceiveShot((3, 4)).Should().Be(ShootResult.Miss);
        fleet.ReceiveShot((4, 3)).Should().Be(ShootResult.Miss);
        fleet.ReceiveShot((4, 4)).Should().Be(ShootResult.Miss);
    }

    [Fact]
    public void shooting_ship_with_two_coordinates()
    {
        var fleet = Fleet.Create(FleetShip.Create((3, 3), (3, 4)));

        fleet.ReceiveShot((3, 3)).Should().Be(ShootResult.Hit);
        fleet.ReceiveShot((3, 4)).Should().Be(ShootResult.Hit);

        fleet.ReceiveShot((3, 2)).Should().Be(ShootResult.Miss);
        fleet.ReceiveShot((3, 5)).Should().Be(ShootResult.Miss);
        fleet.ReceiveShot((2, 3)).Should().Be(ShootResult.Miss);
        fleet.ReceiveShot((4, 3)).Should().Be(ShootResult.Miss);
    }


    [Fact]
    public void shooting_ship_with_many_coordinates()
    {
        var fleet = Fleet.Create(FleetShip.Create((3, 3), (3, 4), (3, 5), (3, 6), (3, 7)));

        fleet.ReceiveShot((3, 4)).Should().Be(ShootResult.Hit);
        fleet.ReceiveShot((3, 5)).Should().Be(ShootResult.Hit);
        fleet.ReceiveShot((3, 6)).Should().Be(ShootResult.Hit);
        fleet.ReceiveShot((3, 7)).Should().Be(ShootResult.Hit);

        fleet.ReceiveShot((3, 2)).Should().Be(ShootResult.Miss);
        fleet.ReceiveShot((2, 4)).Should().Be(ShootResult.Miss);
        fleet.ReceiveShot((4, 5)).Should().Be(ShootResult.Miss);
        fleet.ReceiveShot((3, 9)).Should().Be(ShootResult.Miss);
    }

    [Fact]
    public void shooting_when_there_are_two_ships()
    {
        var fleet = Fleet.Create(
            FleetShip.Create((5, 5), (6, 5)),
            FleetShip.Create((4, 4), (3, 4)));

        fleet.ReceiveShot((5, 5)).Should().Be(ShootResult.Hit);
        fleet.ReceiveShot((3, 4)).Should().Be(ShootResult.Hit);
        
        fleet.ReceiveShot((6, 6)).Should().Be(ShootResult.Miss);
        fleet.ReceiveShot((4, 5)).Should().Be(ShootResult.Miss);
    }

    [Fact]
    public void shooting_when_there_many_ships()
    {
        var fleet = Fleet.Create(
            FleetShip.Create((0, 0), (1, 0), (2, 0)),
            FleetShip.Create((4, 1), (4, 2), (4,3)),
            FleetShip.Create((7, 7)),
            FleetShip.Create((8, 0), (8, 1), (8, 2), (8,3)),
            FleetShip.Create((5, 2), (5, 3), (5,4)));
        
        fleet.ReceiveShot((1, 0)).Should().Be(ShootResult.Hit);
        fleet.ReceiveShot((4, 3)).Should().Be(ShootResult.Hit);
        fleet.ReceiveShot((8, 2)).Should().Be(ShootResult.Hit);
        fleet.ReceiveShot((5, 4)).Should().Be(ShootResult.Hit);
        fleet.ReceiveShot((7, 7)).Should().Be(ShootResult.Hit);
        
        fleet.ReceiveShot((3, 0)).Should().Be(ShootResult.Miss);
        fleet.ReceiveShot((4, 0)).Should().Be(ShootResult.Miss);
        fleet.ReceiveShot((6, 6)).Should().Be(ShootResult.Miss);
        fleet.ReceiveShot((8, 5)).Should().Be(ShootResult.Miss);
    }
}