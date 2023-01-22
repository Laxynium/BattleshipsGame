using Battleships.Console.Fleets;
using Battleships.Console.Matches;
using FluentAssertions;
using static Battleships.UnitTests.Builders.FleetShipBuilder;

namespace Battleships.UnitTests.Matches;

public class MatchesTests
{
    [Fact]
    public void initial_state_is_player_turn_state_which_allows_to_shoot_a_selected_target()
    {
        var fleet = Fleet.Create(CreateShip("1", (3, 3), (3, 4)));
        var match = new Match(fleet);

        var result = match.Handle(new ShootATarget((0, 0)));

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().ContainEquivalentOf(new ShootMissedEvent((0, 0)));
    }

    [Fact]
    public void hitting_a_ship_in_player_turn_state()
    {
        var fleet = Fleet.Create(CreateShip("1", (3, 3), (3, 4)));
        var match = new Match(fleet);

        var result = match.Handle(new ShootATarget((3, 3)));

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().ContainEquivalentOf(new ShootHitShipEvent((3, 3), "1"));
    }

    [Fact]
    public void sinking_a_ship_in_player_turn_state()
    {
        var fleet = Fleet.Create(
            CreateShip("1", (3, 3), (3, 4)),
            CreateShip("2", (1, 1)));
        var match = new Match(fleet);

        var result = match.Handle(new ShootATarget((1, 1)));

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().ContainEquivalentOf(new ShootSunkShipEvent((1, 1), "2"));
    }

    [Fact]
    public void sinking_a_fleet_in_player_turn_state()
    {
        var fleet = Fleet.Create(
            CreateShip("1", (3, 4)));
        var match = new Match(fleet);

        var result = match.Handle(new ShootATarget((3, 4)));

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().ContainEquivalentOf(new ShootSunkFleetEvent((3, 4), "1"));
    }

    [Fact]
    public void missing_a_shoot_in_player_turn_state()
    {
        var fleet = Fleet.Create(
            CreateShip("1", (3, 4), (4, 4)));
        var match = new Match(fleet);

        var result = match.Handle(new ShootATarget((3, 3)));

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().ContainEquivalentOf(new ShootMissedEvent((3, 3)));
    }

    [Fact]
    public void there_is_game_over_when_all_ships_are_sunk()
    {
        var fleet = Fleet.Create(
            CreateShip("1", (5, 5)));
        var match = new Match(fleet);
        match.Handle(new ShootATarget((5, 5)));

        var result = match.Handle(new ShootATarget((6, 6)));

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Contain("Match has ended");
    }
}