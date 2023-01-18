using Battleships.Console;
using FluentAssertions;

namespace Battleships.UnitTests;

public class ShootingFleetTests
{
    [Fact]
    public void for_single_coordinate_ship_there_is_hit_when_shot_coordinates_matches_ship_coordinate()
    {
        Fleet.Create(FleetShip.Create((3, 3)))
            .ReceiveShot((3, 3)).Should().Be(Fleet.ShootResult.Hit);
        
        Fleet.Create(FleetShip.Create((4, 4)))
            .ReceiveShot((4, 4)).Should().Be(Fleet.ShootResult.Hit);
        
        Fleet.Create(FleetShip.Create((5, 7)))
            .ReceiveShot((5, 7)).Should().Be(Fleet.ShootResult.Hit);
    }

    [Fact]
    public void for_single_coordinate_ship_there_is_miss_when_shot_coordinates_is_not_matching_ship_coordinate()
    {
        var fleet = Fleet.Create(FleetShip.Create((3, 3)));

        fleet.ReceiveShot((3, 4)).Should().Be(Fleet.ShootResult.Miss);
        fleet.ReceiveShot((4, 3)).Should().Be(Fleet.ShootResult.Miss);
        fleet.ReceiveShot((4, 4)).Should().Be(Fleet.ShootResult.Miss);
    }

    [Fact]
    public void for_two_coordinates_ship_there_is_hit_when_shot_coordinates_matches_one_of_the_ship_coordinates()
    {
        var fleet = Fleet.Create(FleetShip.Create((3, 3), (3, 4)));

        fleet.ReceiveShot((3, 3)).Should().Be(Fleet.ShootResult.Hit);
        fleet.ReceiveShot((3, 4)).Should().Be(Fleet.ShootResult.Hit);
    }
    
    [Fact]
    public void for_two_coordinates_ship_there_is_miss_when_shot_coordinates_does_not_matches_any_of_the_ship_coordinates()
    {
        var fleet = Fleet.Create(FleetShip.Create((3, 3), (3, 4)));

        fleet.ReceiveShot((3, 2)).Should().Be(Fleet.ShootResult.Miss);
        fleet.ReceiveShot((3, 5)).Should().Be(Fleet.ShootResult.Miss);
        fleet.ReceiveShot((2, 3)).Should().Be(Fleet.ShootResult.Miss);
        fleet.ReceiveShot((4, 3)).Should().Be(Fleet.ShootResult.Miss);
    }
}