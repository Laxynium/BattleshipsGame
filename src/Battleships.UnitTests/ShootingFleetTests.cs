using Battleships.Console;
using FluentAssertions;

namespace Battleships.UnitTests;

public class ShootingFleetTests
{
    [Fact]
    public void for_single_coordinate_ship_there_is_hit_when_shot_coordinates_matches_ship_coordinate()
    {
        Fleet.Create(FleetShip.Create((3, 3)))
            .ReceiveShot((3, 3)).Should().Be(Fleet.ShootResult.GotHit);
        
        Fleet.Create(FleetShip.Create((4, 4)))
            .ReceiveShot((4, 4)).Should().Be(Fleet.ShootResult.GotHit);
        
        Fleet.Create(FleetShip.Create((5, 7)))
            .ReceiveShot((5, 7)).Should().Be(Fleet.ShootResult.GotHit);
    }

    [Fact]
    public void for_single_coordinate_ship_there_is_miss_when_shot_coordinates_is_not_matching_ship_coordinate()
    {
        var fleet = Fleet.Create(FleetShip.Create((3, 3)));

        fleet.ReceiveShot((3, 4)).Should().Be(Fleet.ShootResult.Miss);
        fleet.ReceiveShot((4, 3)).Should().Be(Fleet.ShootResult.Miss);
        fleet.ReceiveShot((4, 4)).Should().Be(Fleet.ShootResult.Miss);
    }
}