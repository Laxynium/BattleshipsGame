using Battleships.Console;
using Battleships.Console.Fleets;
using Battleships.Console.MatchCockpit;
using Battleships.Console.Matches;
using FluentAssertions;

namespace Battleships.UnitTests.MatchCockpit;

public class UpdatingMatchCockpitTests
{
    [Fact]
    public void shoot_miss_event_is_visible_as_white_peg_on_target_board()
    {
        var matchCockpit = new MatchCockpitViewModel(ATargetGrid(new[]
        {
            "x 1 2 3 4",
            "A _ _ @ _",
            "B _ _ @ _",
            "C _ _ _ _",
            "D _ _ _ _",
        }), new List<ShotLog>());
        var matchCockpitUpdater = new MatchCockpitUpdater(matchCockpit);
        
        matchCockpitUpdater.Handle(new ShotMissedEvent(AFleetCoordinates("B4")));

        matchCockpit.TargetGrid.Should().BeEquivalentTo(ATargetGrid(new[]
        {
            "x 1 2 3 4",
            "A _ _ @ _",
            "B _ _ @ @",
            "C _ _ _ _",
            "D _ _ _ _",
        }));
    }

    [Fact]
    public void shot_hit_ship_event_is_visible_as_red_peg_on_target_board()
    {
        var matchCockpit = new MatchCockpitViewModel(ATargetGrid(new[]
        {
            "x 1 2 3 4",
            "A _ ! ! _",
            "B _ _ _ _",
            "C _ _ _ _",
            "D _ _ _ _",
        }), new List<ShotLog>());
        var matchCockpitUpdater = new MatchCockpitUpdater(matchCockpit);
        
        matchCockpitUpdater.Handle(new ShotHitShipEvent(AFleetCoordinates("D1"),"1"));

        matchCockpit.TargetGrid.Should().BeEquivalentTo(ATargetGrid(new[]
        {
            "x 1 2 3 4",
            "A _ ! ! _",
            "B _ _ _ _",
            "C _ _ _ _",
            "D ! _ _ _",
        }));
    }
    
    [Fact]
    public void shot_sunk_ship_event_is_visible_as_red_peg_on_target_board()
    {
        var matchCockpit = new MatchCockpitViewModel(ATargetGrid(new[]
        {
            "x 1 2 3 4",
            "A _ ! _ _",
            "B _ ! _ _",
            "C _ _ _ _",
            "D _ _ _ _",
        }), new List<ShotLog>());
        var matchCockpitUpdater = new MatchCockpitUpdater(matchCockpit);
        
        matchCockpitUpdater.Handle(new ShotSunkShipEvent(AFleetCoordinates("C2"),"1"));

        matchCockpit.TargetGrid.Should().BeEquivalentTo(ATargetGrid(new[]
        {
            "x 1 2 3 4",
            "A _ ! _ _",
            "B _ ! _ _",
            "C _ ! _ _",
            "D _ _ _ _",
        }));
    }
    
    [Fact]
    public void shot_sunk_fleet_event_is_visible_as_red_peg_on_target_board()
    {
        var matchCockpit = new MatchCockpitViewModel(ATargetGrid(new[]
        {
            "x 1 2 3 4",
            "A _ ! _ _",
            "B _ ! _ _",
            "C _ _ _ _",
            "D _ _ _ _",
        }), new List<ShotLog>());
        var matchCockpitUpdater = new MatchCockpitUpdater(matchCockpit);
        
        matchCockpitUpdater.Handle(new ShotSunkFleetEvent(AFleetCoordinates("D1"),"1"));

        matchCockpit.TargetGrid.Should().BeEquivalentTo(ATargetGrid(new[]
        {
            "x 1 2 3 4",
            "A _ ! _ _",
            "B _ ! _ _",
            "C _ _ _ _",
            "D ! _ _ _",
        }));
    }

    [Fact]
    public void log_is_visible_when_shot_misses()
    {
        var matchCockpit = new MatchCockpitViewModel(SomeTargetGrid(),new List<ShotLog>());
        var matchCockpitUpdater = new MatchCockpitUpdater(matchCockpit);
        
        matchCockpitUpdater.Handle(new ShotMissedEvent(AFleetCoordinates("D1")));

        matchCockpit.Logs.Should().ContainInOrder(new ShotLog("D1", "miss",null));
    }
    
    [Fact]
    public void log_is_visible_when_shot_hits_a_ship()
    {
        var matchCockpit = new MatchCockpitViewModel(SomeTargetGrid(),new List<ShotLog>());
        var matchCockpitUpdater = new MatchCockpitUpdater(matchCockpit);
        
        matchCockpitUpdater.Handle(new ShotHitShipEvent(AFleetCoordinates("D3"),new FleetShipId("3")));

        matchCockpit.Logs.Should().ContainInOrder(new ShotLog("D3", "hit","3"));
    }
    
    [Fact]
    public void log_is_visible_when_shot_sunk_a_ship()
    {
        var matchCockpit = new MatchCockpitViewModel(SomeTargetGrid(),new List<ShotLog>());
        var matchCockpitUpdater = new MatchCockpitUpdater(matchCockpit);
        
        matchCockpitUpdater.Handle(new ShotSunkShipEvent(AFleetCoordinates("B3"),new FleetShipId("2")));

        matchCockpit.Logs.Should().ContainInOrder(new ShotLog("B3", "sunk_ship","2"));
    }
    
    [Fact]
    public void log_is_visible_when_shot_sunk_a_fleet()
    {
        var matchCockpit = new MatchCockpitViewModel(SomeTargetGrid(),new List<ShotLog>());
        var matchCockpitUpdater = new MatchCockpitUpdater(matchCockpit);
        
        matchCockpitUpdater.Handle(new ShotSunkFleetEvent(AFleetCoordinates("A4"),new FleetShipId("1")));

        matchCockpit.Logs.Should().ContainInOrder(new ShotLog("A4", "sunk_fleet", "1"));
    }
    
    [Fact]
    public void newest_logs_are_on_the_top_of_list()
    {
        var matchCockpit = new MatchCockpitViewModel(SomeTargetGrid(),new List<ShotLog>());
        var matchCockpitUpdater = new MatchCockpitUpdater(matchCockpit);
        
        matchCockpitUpdater.Handle(new ShotMissedEvent(AFleetCoordinates("A4")));
        matchCockpitUpdater.Handle(new ShotHitShipEvent(AFleetCoordinates("A3"),new FleetShipId("1")));
        matchCockpitUpdater.Handle(new ShotHitShipEvent(AFleetCoordinates("D1"),new FleetShipId("2")));
        matchCockpitUpdater.Handle(new ShotHitShipEvent(AFleetCoordinates("B3"),new FleetShipId("1")));
        matchCockpitUpdater.Handle(new ShotSunkShipEvent(AFleetCoordinates("C3"),new FleetShipId("1")));

        matchCockpit.Logs.Should().ContainInOrder(
            new ShotLog("C3", "sunk_ship", "1"),
            new ShotLog("B3", "hit", "1"),
            new ShotLog("D1", "hit", "2"),
            new ShotLog("A3", "hit", "1"),
            new ShotLog("A4", "miss",null));
    }
    
    private static Coordinates AFleetCoordinates(string gridCoordinates) => 
        GridCoordinates.From(gridCoordinates).ToFleetCoords();

    private static TargetGrid ATargetGrid(IEnumerable<string> lines) =>
        TargetGrid.FromTextRepresentation(lines);

    private static TargetGrid SomeTargetGrid() => ATargetGrid(new[]
    {
        "x 1 2 3 4",
        "A _ _ @ _",
        "B _ _ @ _",
        "C _ _ _ _",
        "D ! ! _ _",
    });

}