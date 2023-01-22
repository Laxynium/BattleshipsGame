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
        }));
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
        }));
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
        }));
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
        }));
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

    private static Coordinates AFleetCoordinates(string gridCoordinates)
    {
        var row = gridCoordinates[0]-'A';
        var column = gridCoordinates[1]-'1';
        return new Coordinates(column, row);
    }
    
    private static TargetGrid ATargetGrid(IEnumerable<string> lines) =>
        TargetGrid.FromTextRepresentation(lines);
}