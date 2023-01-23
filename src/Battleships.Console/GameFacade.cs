using Battleships.Console.MatchCockpit;
using Battleships.Console.MatchConfigurations;
using Battleships.Console.Matches;

namespace Battleships.Console;

public class GameFacade
{
    private const string MatchId = "1";
    private readonly MatchConfiguration _matchConfiguration;
    private readonly IFleetArranger _fleetArranger;

    private readonly MatchRepository _matchRepository = new();
    private readonly MatchViewModelStore _viewModelStore = new();

    public GameFacade(MatchConfiguration matchConfiguration, IFleetArranger fleetArranger)
    {
        _matchConfiguration = matchConfiguration;
        _fleetArranger = fleetArranger;
    }

    public void StartANewMatch()
    {
        var fleet = _matchConfiguration.CreateFleet(_fleetArranger);
        var match = new Match(MatchId, fleet);
        
        _matchRepository.Save(match);
        _viewModelStore.Handle(new MatchStartedEvent(MatchId, _matchConfiguration));
    }

    public void ShootATarget(string gridCoordinates)
    {
        var coordinates = GridCoordinates.From(gridCoordinates).ToFleetCoords();
        if (!_matchConfiguration.AreValid(coordinates))
        {
            throw new ArgumentException("Provided coords are out of grid constraints");
        }

        var match = _matchRepository.Load(MatchId);
        if (match is null)
        {
            throw new InvalidOperationException("Match has to be started first");
        }

        var result = match.Handle(new ShootATarget(coordinates));
        if (result.IsFailure)
        {
            throw new Exception(result.Error);
        }

        _matchRepository.Save(match);
        _viewModelStore.Handle(result.Value);
    }

    public MatchCockpitViewModel GetMatchCockpit()
    {
        var viewModel = _viewModelStore.GetMatchViewModel(MatchId);
        if (viewModel is null)
        {
            throw new InvalidOperationException("Match has to be started first");
        }

        return viewModel.Cockpit;
    }

    public string GetGameState()
    {
        var viewModel = _viewModelStore.GetMatchViewModel(MatchId);
        if (viewModel is null)
        {
            throw new InvalidOperationException("Match has to be started first");
        }

        return viewModel.State;
    }
}