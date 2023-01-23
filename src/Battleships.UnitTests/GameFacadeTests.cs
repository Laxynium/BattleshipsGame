using Battleships.Console;
using Battleships.Console.Fleets;
using Battleships.Console.MatchCockpit;
using Battleships.Console.MatchConfigurations;
using Battleships.UnitTests.MatchConfigurations;
using FluentAssertions;

namespace Battleships.UnitTests;

public class GameFacadeTests
{
    [Fact]
    public void empty_grid_and_no_logs_after_starting_a_game()
    {
        var shipBlueprintsStock = ShipBlueprintsStock.Create(
            ("1", ShipBlueprint.FromText("---")),
            ("2", ShipBlueprint.FromText("--")));
        var matchConfiguration = new MatchConfiguration(
            new GridConstrains(10, 10), shipBlueprintsStock);

        IFleetArranger arranger = new FixedFleetArranger(new []
        {
            (new FleetShipId("1"), new GridCoordinates[]{"D4", "E4", "F4"}),
            (new FleetShipId("2"), new GridCoordinates[]{"C2", "C3"})
        });
        
        var facade = new GameFacade(matchConfiguration, arranger);

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
        var shipBlueprintsStock = ShipBlueprintsStock.Create(
            ("1", ShipBlueprint.FromText("-----")),
            ("2", ShipBlueprint.FromText("----")),
            ("3", ShipBlueprint.FromText("----")));
        var matchConfiguration = new MatchConfiguration(
            new GridConstrains(10, 10), shipBlueprintsStock);

        IFleetArranger arranger = new FixedFleetArranger(new []
        {
            (new FleetShipId("1"), new GridCoordinates[]{"B1", "B2", "B3", "B4", "B5"}),
            (new FleetShipId("2"), new GridCoordinates[]{"C2", "D2", "E2", "F2"}),
            (new FleetShipId("3"), new GridCoordinates[]{"C5", "D5", "E5", "F5"}),
        });
        
        var facade = new GameFacade(matchConfiguration, arranger);
        facade.StartANewMatch();

        facade.ShootATarget("E4");
        facade.ShootATarget("D5");
        facade.ShootATarget("E5");

        var cockpit = facade.GetMatchCockpit();
        cockpit.TargetGrid.Should().BeEquivalentTo(TargetGrid.FromTextRepresentation(new[]
        {
            "x 1 2 3 4 5 6 7 8 9 10",
            "A _ _ _ _ _ _ _ _ _ _",
            "B _ _ _ _ _ _ _ _ _ _",
            "C _ _ _ _ _ _ _ _ _ _",
            "D _ _ _ _ ! _ _ _ _ _",
            "E _ _ _ @ ! _ _ _ _ _",
            "F _ _ _ _ _ _ _ _ _ _",
            "G _ _ _ _ _ _ _ _ _ _",
            "H _ _ _ _ _ _ _ _ _ _",
            "I _ _ _ _ _ _ _ _ _ _",
            "J _ _ _ _ _ _ _ _ _ _",
        }));
        cockpit.Logs.Should().ContainInOrder(
            new ShotLog("E5",ShotResultDto.Hit, "3"),
            new ShotLog("D5", ShotResultDto.Hit, "3"),
            new ShotLog("E4",ShotResultDto.Miss,null));
    }

    [Fact]
    public void starting_a_new_game_restarts_grid_and_fleet_state()
    {
        var shipBlueprintsStock = ShipBlueprintsStock.Create(
            ("1", ShipBlueprint.FromText("-----")),
            ("2", ShipBlueprint.FromText("----")),
            ("3", ShipBlueprint.FromText("----")));
        var matchConfiguration = new MatchConfiguration(
            new GridConstrains(10, 10), shipBlueprintsStock);

        IFleetArranger arranger = new FixedFleetArranger(new []
        {
            (new FleetShipId("1"), new GridCoordinates[]{"B1", "B2", "B3", "B4", "B5"}),
            (new FleetShipId("2"), new GridCoordinates[]{"C2", "D2", "E2", "F2"}),
            (new FleetShipId("3"), new GridCoordinates[]{"C5", "D5", "E5", "F5"}),
        });
        
        var facade = new GameFacade(matchConfiguration, arranger);
        facade.StartANewMatch();

        facade.ShootATarget("C5");
        facade.ShootATarget("D5");
        facade.ShootATarget("E5");
        
        facade.StartANewMatch();
        facade.ShootATarget("F5");
        
        var cockpit = facade.GetMatchCockpit();
        cockpit.TargetGrid.Should().BeEquivalentTo(TargetGrid.FromTextRepresentation(new[]
        {
            "x 1 2 3 4 5 6 7 8 9 10",
            "A _ _ _ _ _ _ _ _ _ _",
            "B _ _ _ _ _ _ _ _ _ _",
            "C _ _ _ _ _ _ _ _ _ _",
            "D _ _ _ _ _ _ _ _ _ _",
            "E _ _ _ _ _ _ _ _ _ _",
            "F _ _ _ _ ! _ _ _ _ _",
            "G _ _ _ _ _ _ _ _ _ _",
            "H _ _ _ _ _ _ _ _ _ _",
            "I _ _ _ _ _ _ _ _ _ _",
            "J _ _ _ _ _ _ _ _ _ _",
        }));
        cockpit.Logs.Should().ContainInOrder(
            new ShotLog("F5", ShotResultDto.Hit, "3"));
    }

    [Fact]
    public void match_cockpit_matches_the_size_specified_in_configuration()
    {
        var shipBlueprintsStock = ShipBlueprintsStock.Create(
            ("1", ShipBlueprint.FromText("-----")),
            ("2", ShipBlueprint.FromText("----")),
            ("3", ShipBlueprint.FromText("----")));
        var matchConfiguration = new MatchConfiguration(
            new GridConstrains(5, 5), shipBlueprintsStock);

        IFleetArranger arranger = new FixedFleetArranger(new []
        {
            (new FleetShipId("1"), new GridCoordinates[]{"B1", "B2", "B3", "B4", "B5"}),
            (new FleetShipId("2"), new GridCoordinates[]{"A1", "A2", "A3", "A4"}),
            (new FleetShipId("3"), new GridCoordinates[]{"C1", "C2", "C3", "C4"}),
        });
        
        var facade = new GameFacade(matchConfiguration, arranger);
        facade.StartANewMatch();
        
        var cockpit = facade.GetMatchCockpit();
        cockpit.TargetGrid.Should().BeEquivalentTo(TargetGrid.FromTextRepresentation(new[]
        {
            "x 1 2 3 4 5",
            "A _ _ _ _ _",
            "B _ _ _ _ _",
            "C _ _ _ _ _",
            "D _ _ _ _ _",
            "E _ _ _ _ _"
        }));
        cockpit.Logs.Should().BeEmpty();
    }
}