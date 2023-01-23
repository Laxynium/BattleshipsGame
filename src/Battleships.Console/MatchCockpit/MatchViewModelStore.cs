﻿using Battleships.Console.MatchConfigurations;
using Battleships.Console.Matches;

namespace Battleships.Console.MatchCockpit;

public class MatchViewModelStore :
    IMatchEventHandler<MatchStartedEvent>,
    IMatchEventHandler<MatchOverEvent>
{
    private readonly Dictionary<string, MatchViewModel> _viewModels = new();

    public MatchViewModel? GetMatchViewModel(string matchId)
    {
        if (!_viewModels.ContainsKey(matchId))
        {
            return null;
        }
        
        return _viewModels[matchId];
    }

    public void Handle(IEnumerable<IMatchEvent> matchEvents)
    {
        foreach (var matchEvent in matchEvents)
        {
            Handle(matchEvent);
        }
    }
    
    public void Handle(IMatchEvent matchEvent)
    {
        switch (matchEvent)
        {
            case MatchStartedEvent e:
                Handle(e);
                break;
            case MatchOverEvent e:
                Handle(e);
                break;
            default:
                new MatchCockpitUpdater(_viewModels["1"].Cockpit)
                    .Handle(matchEvent);
                break;
        }
    }


    public void Handle(MatchStartedEvent @event)
    {
        _viewModels[@event.MatchId] = new MatchViewModel
        {
            Cockpit = InitialMatchCockpit(@event.MatchConfiguration),
            State = "player_turn"
        };
    }

    public void Handle(MatchOverEvent @event)
    {
        _viewModels[@event.MatchId].State = "match_over";
    }

    private static MatchCockpitViewModel InitialMatchCockpit(MatchConfiguration matchConfiguration) =>
        new(new TargetGrid(
                Enumerable.Range(0, matchConfiguration.Constrains.Height).Select(y => 
                        Enumerable.Range(0, matchConfiguration.Constrains.Width).Select(x => Cell.None).ToArray())
                    .ToArray()),
            new List<ShotLog>());
}