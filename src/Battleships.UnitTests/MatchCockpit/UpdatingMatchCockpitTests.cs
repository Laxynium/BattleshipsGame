using Battleships.Console.Application;
using Battleships.Console.Application.Fleets;
using Battleships.Console.Application.MatchCockpit;
using Battleships.Console.Application.Matches;
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
        var matchCockpitUpdater = AMatchCockpitUpdater(matchCockpit);

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
        var matchCockpitUpdater = AMatchCockpitUpdater(matchCockpit);

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
        var matchCockpitUpdater = AMatchCockpitUpdater(matchCockpit);

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
        var matchCockpitUpdater = AMatchCockpitUpdater(matchCockpit);

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
        var matchCockpitUpdater = AMatchCockpitUpdater(matchCockpit);

        matchCockpitUpdater.Handle(new ShotMissedEvent("1", AFleetCoordinates("D1")));

        matchCockpit.Logs.Should().ContainInOrder(new ShotLog("D1", ShotResultDto.Miss, null, null));
    }

    [Fact]
    public void log_is_visible_when_shot_hits_a_ship()
    {
        var matchCockpit = new MatchCockpitViewModel(SomeTargetGrid(), new List<ShotLog>());
        var matchCockpitUpdater = AMatchCockpitUpdater(matchCockpit, ("1", "Destroyer"), ("2", "Submarine"), ("3", "Cruiser"));

        matchCockpitUpdater.Handle(new ShotHitShipEvent("1", AFleetCoordinates("D3"), new FleetShipId("3")));

        matchCockpit.Logs.Should().ContainInOrder(new ShotLog("D3", ShotResultDto.Hit, "3", "Cruiser"));
    }

    [Fact]
    public void log_is_visible_when_shot_sunk_a_ship()
    {
        var matchCockpit = new MatchCockpitViewModel(SomeTargetGrid(), new List<ShotLog>());
        var matchCockpitUpdater = AMatchCockpitUpdater(matchCockpit, ("1", "Destroyer"), ("2", "Submarine"));

        matchCockpitUpdater.Handle(new ShotSunkShipEvent("1", AFleetCoordinates("B3"), new FleetShipId("2")));

        matchCockpit.Logs.Should().ContainInOrder(new ShotLog("B3", ShotResultDto.SunkShip, "2", "Submarine"));
    }

    [Fact]
    public void log_is_visible_when_shot_sunk_a_fleet()
    {
        var matchCockpit = new MatchCockpitViewModel(SomeTargetGrid(), new List<ShotLog>());
        var matchCockpitUpdater = AMatchCockpitUpdater(matchCockpit, ("1", "Destroyer") );

        matchCockpitUpdater.Handle(new ShotSunkFleetEvent("1", AFleetCoordinates("A4"), new FleetShipId("1")));

        matchCockpit.Logs.Should().ContainInOrder(new ShotLog("A4", ShotResultDto.SunkFleet, "1","Destroyer"));
    }

    [Fact]
    public void newest_logs_are_on_the_top_of_list()
    {
        var matchCockpit = new MatchCockpitViewModel(SomeTargetGrid(), new List<ShotLog>());
        var matchCockpitUpdater = AMatchCockpitUpdater(matchCockpit, ("1", "Destroyer"), ("2", "Submarine"));

        matchCockpitUpdater.Handle(new ShotMissedEvent("1", AFleetCoordinates("A4")));
        matchCockpitUpdater.Handle(new ShotHitShipEvent("1", AFleetCoordinates("A3"), new FleetShipId("1")));
        matchCockpitUpdater.Handle(new ShotHitShipEvent("1", AFleetCoordinates("D1"), new FleetShipId("2")));
        matchCockpitUpdater.Handle(new ShotHitShipEvent("1", AFleetCoordinates("B3"), new FleetShipId("1")));
        matchCockpitUpdater.Handle(new ShotSunkShipEvent("1", AFleetCoordinates("C3"), new FleetShipId("1")));

        matchCockpit.Logs.Should().ContainInOrder(
            new ShotLog("C3", ShotResultDto.SunkShip, "1", "Destroyer"),
            new ShotLog("B3", ShotResultDto.Hit, "1", "Destroyer"),
            new ShotLog("D1", ShotResultDto.Hit, "2", "Submarine"),
            new ShotLog("A3", ShotResultDto.Hit, "1", "Destroyer"),
            new ShotLog("A4", ShotResultDto.Miss, null, null));
    }

    private static MatchCockpitUpdater AMatchCockpitUpdater(MatchCockpitViewModel matchCockpit) => 
        new(matchCockpit, new MatchConfigurationDto(new Dictionary<string, string>()));

    private static MatchCockpitUpdater AMatchCockpitUpdater(MatchCockpitViewModel matchCockpit, params (string id, string name)[] shipsNames) => 
        new(matchCockpit, new MatchConfigurationDto(shipsNames.ToDictionary(x=>x.id, x=>x.name)));
    
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