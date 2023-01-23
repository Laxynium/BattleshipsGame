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

        matchCockpitUpdater.Handle(new ShotMissedEvent("1", AFleetCoordinates("B4")));

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

        matchCockpitUpdater.Handle(new ShotHitShipEvent("1", AFleetCoordinates("D1"), "1"));

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

        matchCockpitUpdater.Handle(new ShotSunkShipEvent("1", AFleetCoordinates("C2"), "1"));

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

        matchCockpitUpdater.Handle(new ShotSunkFleetEvent("1", AFleetCoordinates("D1"), "1"));

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
        var matchCockpit = new MatchCockpitViewModel(SomeTargetGrid(), new List<ShotLog>());
        var matchCockpitUpdater = new MatchCockpitUpdater(matchCockpit);

        matchCockpitUpdater.Handle(new ShotMissedEvent("1", AFleetCoordinates("D1")));

        matchCockpit.Logs.Should().ContainInOrder(new ShotLog("D1", ShotResultDto.Miss, null));
    }

    [Fact]
    public void log_is_visible_when_shot_hits_a_ship()
    {
        var matchCockpit = new MatchCockpitViewModel(SomeTargetGrid(), new List<ShotLog>());
        var matchCockpitUpdater = new MatchCockpitUpdater(matchCockpit);

        matchCockpitUpdater.Handle(new ShotHitShipEvent("1", AFleetCoordinates("D3"), new FleetShipId("3")));

        matchCockpit.Logs.Should().ContainInOrder(new ShotLog("D3", ShotResultDto.Hit, "3"));
    }

    [Fact]
    public void log_is_visible_when_shot_sunk_a_ship()
    {
        var matchCockpit = new MatchCockpitViewModel(SomeTargetGrid(), new List<ShotLog>());
        var matchCockpitUpdater = new MatchCockpitUpdater(matchCockpit);

        matchCockpitUpdater.Handle(new ShotSunkShipEvent("1", AFleetCoordinates("B3"), new FleetShipId("2")));

        matchCockpit.Logs.Should().ContainInOrder(new ShotLog("B3", ShotResultDto.SunkShip, "2"));
    }

    [Fact]
    public void log_is_visible_when_shot_sunk_a_fleet()
    {
        var matchCockpit = new MatchCockpitViewModel(SomeTargetGrid(), new List<ShotLog>());
        var matchCockpitUpdater = new MatchCockpitUpdater(matchCockpit);

        matchCockpitUpdater.Handle(new ShotSunkFleetEvent("1", AFleetCoordinates("A4"), new FleetShipId("1")));

        matchCockpit.Logs.Should().ContainInOrder(new ShotLog("A4", ShotResultDto.SunkFleet, "1"));
    }

    [Fact]
    public void newest_logs_are_on_the_top_of_list()
    {
        var matchCockpit = new MatchCockpitViewModel(SomeTargetGrid(), new List<ShotLog>());
        var matchCockpitUpdater = new MatchCockpitUpdater(matchCockpit);

        matchCockpitUpdater.Handle(new ShotMissedEvent("1", AFleetCoordinates("A4")));
        matchCockpitUpdater.Handle(new ShotHitShipEvent("1", AFleetCoordinates("A3"), new FleetShipId("1")));
        matchCockpitUpdater.Handle(new ShotHitShipEvent("1", AFleetCoordinates("D1"), new FleetShipId("2")));
        matchCockpitUpdater.Handle(new ShotHitShipEvent("1", AFleetCoordinates("B3"), new FleetShipId("1")));
        matchCockpitUpdater.Handle(new ShotSunkShipEvent("1", AFleetCoordinates("C3"), new FleetShipId("1")));

        matchCockpit.Logs.Should().ContainInOrder(
            new ShotLog("C3", ShotResultDto.SunkShip, "1"),
            new ShotLog("B3", ShotResultDto.Hit, "1"),
            new ShotLog("D1", ShotResultDto.Hit, "2"),
            new ShotLog("A3", ShotResultDto.Hit, "1"),
            new ShotLog("A4", ShotResultDto.Miss, null));
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