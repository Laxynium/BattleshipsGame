using Battleships.Console;
using Battleships.Console.Fleets;
using Battleships.Console.MatchCockpit;
using FluentAssertions;
using static Battleships.UnitTests.Builders.FleetShipBuilder;

namespace Battleships.UnitTests;

public class GameFacadeTests
{
    [Fact]
    public void empty_grid_and_no_logs_after_starting_a_game()
    {
        var fleet = Fleet.Create(
            CreateShip("1",(3,3),(3,4),(3,5)),
            CreateShip("2", (1,2),(2,2)));
        var facade = new GameFacade(fleet);

        facade.StartANewMatch();

        var cockpit = facade.GetMatchCockpit();
        cockpit.TargetGrid.Should().BeEquivalentTo(TargetGrid.FromTextRepresentation(new[]
        {
            "x 1 2 3 4 5 6 7 8 9 10",
            "A _ _ _ _ _ _ _ _ _ _",
            "B _ _ _ _ _ _ _ _ _ _",
            "C _ _ _ _ _ _ _ _ _ _",
            "D _ _ _ _ _ _ _ _ _ _",
            "E _ _ _ _ _ _ _ _ _ _",
            "F _ _ _ _ _ _ _ _ _ _",
            "G _ _ _ _ _ _ _ _ _ _",
            "H _ _ _ _ _ _ _ _ _ _",
            "I _ _ _ _ _ _ _ _ _ _",
            "J _ _ _ _ _ _ _ _ _ _",
        }));
        cockpit.Logs.Should().BeEmpty();
    }
    
    
    [Fact]
    public void after_starting_a_game_player_can_shoot_a_target()
    {
        var fleet = Fleet.Create(
            CreateShip("1",(4,3),(4,4),(4,5)),
            CreateShip("2", (1,2),(2,2)));
        var facade = new GameFacade(fleet);
        facade.StartANewMatch();

        facade.ShootATarget("E4");

        var cockpit = facade.GetMatchCockpit();
        cockpit.TargetGrid.Should().BeEquivalentTo(TargetGrid.FromTextRepresentation(new[]
        {
            "x 1 2 3 4 5 6 7 8 9 10",
            "A _ _ _ _ _ _ _ _ _ _",
            "B _ _ _ _ _ _ _ _ _ _",
            "C _ _ _ _ _ _ _ _ _ _",
            "D _ _ _ _ _ _ _ _ _ _",
            "E _ _ _ @ _ _ _ _ _ _",
            "F _ _ _ _ _ _ _ _ _ _",
            "G _ _ _ _ _ _ _ _ _ _",
            "H _ _ _ _ _ _ _ _ _ _",
            "I _ _ _ _ _ _ _ _ _ _",
            "J _ _ _ _ _ _ _ _ _ _",
        }));
        cockpit.Logs.Should().Contain(
            new ShotLog("E4","miss",null));
        
        
        facade.ShootATarget("D5");
        cockpit = facade.GetMatchCockpit();
        cockpit.TargetGrid.Should().BeEquivalentTo(TargetGrid.FromTextRepresentation(new[]
        {
            "x 1 2 3 4 5 6 7 8 9 10",
            "A _ _ _ _ _ _ _ _ _ _",
            "B _ _ _ _ _ _ _ _ _ _",
            "C _ _ _ _ _ _ _ _ _ _",
            "D _ _ _ _ ! _ _ _ _ _",
            "E _ _ _ @ _ _ _ _ _ _",
            "F _ _ _ _ _ _ _ _ _ _",
            "G _ _ _ _ _ _ _ _ _ _",
            "H _ _ _ _ _ _ _ _ _ _",
            "I _ _ _ _ _ _ _ _ _ _",
            "J _ _ _ _ _ _ _ _ _ _",
        }));
        cockpit.Logs.Should().Contain(
            new ShotLog("D5","hit","1"));
    }
}