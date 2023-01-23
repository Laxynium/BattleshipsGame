using Battleships.Console.Application.MatchConfigurations;
using Battleships.Console.Application.Matches;

namespace Battleships.Console.Application.MatchCockpit;

public class MatchViewModelStore
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

    private void Handle(MatchEvent matchEvent)
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
                new MatchCockpitUpdater(_viewModels[matchEvent.MatchId].Cockpit, _viewModels[matchEvent.MatchId].Configuration)
                    .Handle(matchEvent);
                break;
        }
    }


    public void Handle(MatchStartedEvent @event)
    {
        _viewModels[@event.MatchId] =
            new MatchViewModel(InitialMatchCockpit(@event.MatchConfiguration),
                MatchStateDto.PlayerTurn,
                new MatchConfigurationDto(GetShipsNames(@event)));
    }

    public void Handle(MatchOverEvent @event)
    {
        _viewModels[@event.MatchId] = _viewModels[@event.MatchId] with { State = MatchStateDto.MatchOver };
    }

    private static MatchCockpitViewModel InitialMatchCockpit(MatchConfiguration matchConfiguration) =>
        new(new TargetGrid(
                Enumerable.Range(0, matchConfiguration.Constrains.Height).Select(y =>
                        Enumerable.Range(0, matchConfiguration.Constrains.Width).Select(x => Cell.None).ToArray())
                    .ToArray()),
            new List<ShotLog>());

    private static Dictionary<string, string> GetShipsNames(MatchStartedEvent @event)
    {
        var shipsNames = @event.MatchConfiguration.BlueprintsStock.ShipBlueprints
            .ToDictionary(x => x.id.Value,
                x => x.shipBlueprint.ShipBlueprintName.Name);
        return shipsNames;
    }
}