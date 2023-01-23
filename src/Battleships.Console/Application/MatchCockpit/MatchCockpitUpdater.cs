using Battleships.Console.Application.Fleets;
using Battleships.Console.Application.Matches;

namespace Battleships.Console.Application.MatchCockpit;

public class MatchCockpitUpdater :
    IMatchEventHandler<ShotSunkFleetEvent>,
    IMatchEventHandler<ShotSunkShipEvent>,
    IMatchEventHandler<ShotHitShipEvent>,
    IMatchEventHandler<ShotMissedEvent>
{
    private readonly MatchCockpitViewModel _matchCockpitViewModel;
    private readonly MatchConfigurationDto _matchConfiguration;

    public MatchCockpitUpdater(MatchCockpitViewModel matchCockpitViewModel, MatchConfigurationDto matchConfiguration)
    {
        _matchCockpitViewModel = matchCockpitViewModel;
        _matchConfiguration = matchConfiguration;
    }

    public void Handle(MatchEvent matchEvent)
    {
        switch (matchEvent)
        {
            case ShotSunkFleetEvent e:
                Handle(e);
                break;
            case ShotSunkShipEvent e:
                Handle(e);
                break;
            case ShotHitShipEvent e:
                Handle(e);
                break;
            case ShotMissedEvent e:
                Handle(e);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(matchEvent));
        }
    }

    public void Handle(ShotSunkFleetEvent @event)
    {
        PlaceAPeg(@event.Coordinates, Cell.RedPeg);
        WriteALog(@event.Coordinates, ShotResultDto.SunkFleet, @event.FleetShipId.Value);
    }

    public void Handle(ShotSunkShipEvent @event)
    {
        PlaceAPeg(@event.Coordinates, Cell.RedPeg);
        WriteALog(@event.Coordinates, ShotResultDto.SunkShip, @event.FleetShipId.Value);
    }

    public void Handle(ShotHitShipEvent @event)
    {
        PlaceAPeg(@event.Coordinates, Cell.RedPeg);
        WriteALog(@event.Coordinates, ShotResultDto.Hit, @event.FleetShipId.Value);
    }

    public void Handle(ShotMissedEvent @event)
    {
        PlaceAPeg(@event.Coordinates, Cell.WhitePeg);
        WriteALog(@event.Coordinates, ShotResultDto.Miss, null);
    }

    private void WriteALog(Coordinates coordinates, ShotResultDto shotResult, string? shipId)
    {
        var gridCoordinates = GridCoordinates.From(coordinates).ToString();
        _matchCockpitViewModel.Logs.Insert(0,CreateShotLog(gridCoordinates, shotResult, shipId));
    }

    private ShotLog CreateShotLog(string gridCoordinates, ShotResultDto shotResult, string? shipId) =>
        new(gridCoordinates,
            shotResult,
            shipId,
            GetShipName(shipId)
        );

    private string? GetShipName(string? shipId) =>
        shipId != null && _matchConfiguration.ShipsNames.ContainsKey(shipId)
            ? _matchConfiguration.ShipsNames[shipId] 
            : null;

    private void PlaceAPeg(Coordinates coordinates, Cell peg)
    {
        var (x, y) = coordinates;
        _matchCockpitViewModel.TargetGrid.Cells[y][x] = peg;
    }
}