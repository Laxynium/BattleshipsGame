﻿using Battleships.Console.Fleets;
using Battleships.Console.Matches;

namespace Battleships.Console.MatchCockpit;

public class MatchCockpitUpdater : 
    IMatchEventHandler<ShotSunkFleetEvent>, 
    IMatchEventHandler<ShotSunkShipEvent>,
    IMatchEventHandler<ShotHitShipEvent>,
    IMatchEventHandler<ShotMissedEvent>
{
    private readonly MatchCockpitViewModel _matchCockpitViewModel;

    public MatchCockpitUpdater(MatchCockpitViewModel matchCockpitViewModel)
    {
        _matchCockpitViewModel = matchCockpitViewModel;
    }

    public void Handle(IMatchEvent matchEvent)
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
        WriteALog(@event.Coordinates, "sunk_fleet", @event.FleetShipId.Value);
    }

    public void Handle(ShotSunkShipEvent @event)
    {
        PlaceAPeg(@event.Coordinates, Cell.RedPeg);
        WriteALog(@event.Coordinates, "sunk_ship", @event.FleetShipId.Value);
    }

    public void Handle(ShotHitShipEvent @event)
    {
        PlaceAPeg(@event.Coordinates, Cell.RedPeg);
        WriteALog(@event.Coordinates, "hit", @event.FleetShipId.Value);
    }

    public void Handle(ShotMissedEvent @event)
    {
        PlaceAPeg(@event.Coordinates, Cell.WhitePeg);
        WriteALog(@event.Coordinates, "miss", null);
    }

    private void WriteALog(Coordinates coordinates, string shotResult, string? shipId)
    {
        var gridCoordinates = CoordinatesTranslator.AGridCoordinates(coordinates);
        _matchCockpitViewModel.Logs.Insert(0,new ShotLog(gridCoordinates, shotResult, shipId));
    }

    private void PlaceAPeg(Coordinates coordinates, Cell peg)
    {
        var (x, y) = coordinates;
        _matchCockpitViewModel.TargetGrid.Cells[y][x] = peg;
    }
}