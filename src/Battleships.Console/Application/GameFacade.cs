using Battleships.Console.Application.MatchCockpit;
using Battleships.Console.Application.MatchConfigurations;
using Battleships.Console.Application.Matches;
using CSharpFunctionalExtensions;

namespace Battleships.Console.Application;

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

    public IUnitResult<BattleshipsErrorResponse> ShootATarget(string gridCoordinates)
    {
        if (string.IsNullOrWhiteSpace(gridCoordinates))
        {
            return UnitResult.Failure(new BattleshipsErrorResponse("Target coordinates cannot be empty"));
        }

        var fleetCoordinates = GridCoordinates
            .Parse(gridCoordinates)
            .Map(x => x.ToFleetCoords());

        if (fleetCoordinates.IsFailure)
        {
            return UnitResult.Failure(new BattleshipsErrorResponse(fleetCoordinates.Error));
        }
        
        if (!_matchConfiguration.AreValid(fleetCoordinates.Value))
        {
            return UnitResult.Failure(new BattleshipsErrorResponse("Target coordinates must be inside grid"));
        }

        var match = _matchRepository.Load(MatchId);
        if (match is null)
        {
            return UnitResult.Failure(new BattleshipsErrorResponse("Started match was not found"));
        }

        var result = match.Handle(new ShootATarget(fleetCoordinates.Value));
        if (result.IsFailure)
        {
            return UnitResult.Failure(new BattleshipsErrorResponse(result.Error));
        }

        _matchRepository.Save(match);
        _viewModelStore.Handle(result.Value);
        
        return UnitResult.Success<BattleshipsErrorResponse>();
    }

    public MatchCockpitViewModel? GetMatchCockpit() => 
        _viewModelStore.GetMatchViewModel(MatchId)?.Cockpit;

    public MatchStateDto GetGameState()
    {
        var viewModel = _viewModelStore.GetMatchViewModel(MatchId);
        return viewModel is null ? MatchStateDto.WaitingForMatch : viewModel.State;
    }
}