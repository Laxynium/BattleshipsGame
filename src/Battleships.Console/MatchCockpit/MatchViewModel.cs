using Battleships.Console.MatchConfigurations;
using Battleships.Console.Matches;

namespace Battleships.Console.MatchCockpit;

public record MatchViewModel
{
    public MatchCockpitViewModel Cockpit { get; set; }
    public string State { get; set; } = "waiting_for_game";
}

public class MatchViewModelUpdater :
    IMatchEventHandler<MatchStartedEvent>,
    IMatchEventHandler<MatchOverEvent>
{
    private readonly MatchViewModel _matchViewModel;
    private MatchCockpitUpdater _matchCockpitUpdater;

    public MatchViewModelUpdater(MatchViewModel matchViewModel)
    {
        _matchViewModel = matchViewModel;
        _matchCockpitUpdater = new MatchCockpitUpdater(_matchViewModel.Cockpit);
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
                _matchCockpitUpdater.Handle(matchEvent);
                break;
        }
    }
    
    public void Handle(MatchStartedEvent @event)
    {
        _matchViewModel.Cockpit = InitialMatchCockpit(@event.MatchConfiguration);
        _matchCockpitUpdater = new MatchCockpitUpdater(_matchViewModel.Cockpit);
        _matchViewModel.State = "player_turn";
    }

    public void Handle(MatchOverEvent @event)
    {
        _matchViewModel.State = "match_over";
    }
    
    private static MatchCockpitViewModel InitialMatchCockpit(MatchConfiguration matchConfiguration) =>
        new(new TargetGrid(
                Enumerable.Range(0, matchConfiguration.Constrains.Height).Select(y => 
                        Enumerable.Range(0, matchConfiguration.Constrains.Width).Select(x => Cell.None).ToArray())
                    .ToArray()),
            new List<ShotLog>());
}