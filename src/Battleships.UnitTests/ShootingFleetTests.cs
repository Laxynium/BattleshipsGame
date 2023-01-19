using Battleships.Console;
using FluentAssertions;

namespace Battleships.UnitTests;

public class ShootingFleetTests
{
    [Fact]
    public void shooting_ship_with_one_coordinate_when_is_hit()
    {
        Fleet.Create(FleetShip.Create((3, 3)))
            .ReceiveShot((3, 3)).Should().Be(Fleet.ShootResult.Hit);
        
        Fleet.Create(FleetShip.Create((4, 4)))
            .ReceiveShot((4, 4)).Should().Be(Fleet.ShootResult.Hit);
        
        Fleet.Create(FleetShip.Create((5, 7)))
            .ReceiveShot((5, 7)).Should().Be(Fleet.ShootResult.Hit);
    }

    [Fact]
    public void shooting_ship_with_one_coordinate_when_is_miss()
    {
        var fleet = Fleet.Create(FleetShip.Create((3, 3)));

        fleet.ReceiveShot((3, 4)).Should().Be(Fleet.ShootResult.Miss);
        fleet.ReceiveShot((4, 3)).Should().Be(Fleet.ShootResult.Miss);
        fleet.ReceiveShot((4, 4)).Should().Be(Fleet.ShootResult.Miss);
    }

    [Fact]
    public void shooting_ship_with_two_coordinates()
    {
        var fleet = Fleet.Create(FleetShip.Create((3, 3), (3, 4)));

        fleet.ReceiveShot((3, 3)).Should().Be(Fleet.ShootResult.Hit);
        fleet.ReceiveShot((3, 4)).Should().Be(Fleet.ShootResult.Hit);
        
        fleet.ReceiveShot((3, 2)).Should().Be(Fleet.ShootResult.Miss);
        fleet.ReceiveShot((3, 5)).Should().Be(Fleet.ShootResult.Miss);
        fleet.ReceiveShot((2, 3)).Should().Be(Fleet.ShootResult.Miss);
        fleet.ReceiveShot((4, 3)).Should().Be(Fleet.ShootResult.Miss);
    }
    

    [Fact]
    public void shooting_ship_with_many_coordinates()
    {
        var fleet = Fleet.Create(FleetShip.Create((3, 3), (3, 4), (3, 5), (3, 6), (3, 7)));

        fleet.ReceiveShot((3, 4)).Should().Be(Fleet.ShootResult.Hit);
        fleet.ReceiveShot((3, 5)).Should().Be(Fleet.ShootResult.Hit);
        fleet.ReceiveShot((3, 6)).Should().Be(Fleet.ShootResult.Hit);
        fleet.ReceiveShot((3, 7)).Should().Be(Fleet.ShootResult.Hit);

        fleet.ReceiveShot((3, 2)).Should().Be(Fleet.ShootResult.Miss);
        fleet.ReceiveShot((2, 4)).Should().Be(Fleet.ShootResult.Miss);
        fleet.ReceiveShot((4, 5)).Should().Be(Fleet.ShootResult.Miss);
        fleet.ReceiveShot((3, 9)).Should().Be(Fleet.ShootResult.Miss);
    }

    [Fact]
    public void shooting_when_there_are_two_ships()
    {
        var fleet = Fleet.Create(
            FleetShip.Create((5, 5), (6, 5)),
            FleetShip.Create((4,4),(3,4)));

        fleet.ReceiveShot((5, 5)).Should().Be(Fleet.ShootResult.Hit);
        fleet.ReceiveShot((3, 4)).Should().Be(Fleet.ShootResult.Hit);
    }
}