using Battleships.Console.Application.MatchConfigurations;
using Battleships.Console.Application.Matches;

namespace Battleships.Console.Application.MatchCockpit;

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

    public void Handle(IEnumerable<MatchEvent> matchEvents)
    {
        foreach (var matchEvent in matchEvents)
        {
            Handle(matchEvent);
        }
    }
    
    public void Handle(MatchEvent matchEvent)
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
                new MatchCockpitUpdater(_viewModels[matchEvent.MatchId].Cockpit)
                    .Handle(matchEvent);
                break;
        }
    }


    public void Handle(MatchStartedEvent @event)
    {
        _viewModels[@event.MatchId] = new MatchViewModel(InitialMatchCockpit(@event.MatchConfiguration), MatchStateDto.PlayerTurn)
        {
            Cockpit = InitialMatchCockpit(@event.MatchConfiguration),
            State = MatchStateDto.PlayerTurn
        };
    }

    public void Handle(MatchOverEvent @event)
    {
        _viewModels[@event.MatchId] = _viewModels[@event.MatchId] with{State = MatchStateDto.MatchOver};
    }

    private static MatchCockpitViewModel InitialMatchCockpit(MatchConfiguration matchConfiguration) =>
        new(new TargetGrid(
                Enumerable.Range(0, matchConfiguration.Constrains.Height).Select(y => 
                        Enumerable.Range(0, matchConfiguration.Constrains.Width).Select(x => Cell.None).ToArray())
                    .ToArray()),
            new List<ShotLog>());
}