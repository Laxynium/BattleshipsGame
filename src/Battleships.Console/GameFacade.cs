 using Battleships.Console.MatchCockpit;
using Battleships.Console.MatchConfigurations;
using Battleships.Console.Matches;

namespace Battleships.Console;

public class GameFacade
{
    private readonly MatchConfiguration _matchConfiguration;
    private readonly IFleetArranger _fleetArranger;

    private Match? _match;
    private MatchViewModel? _matchViewModel;
    private MatchViewModelUpdater? _matchViewModelUpdater;
    
    public GameFacade(MatchConfiguration matchConfiguration, IFleetArranger fleetArranger)
    {
        _matchConfiguration = matchConfiguration;
        _fleetArranger = fleetArranger;
    }
    
    public void StartANewMatch()
    {
        _matchViewModel = new MatchViewModel();
        _matchViewModelUpdater = new MatchViewModelUpdater(_matchViewModel);
        
        var fleet = _matchConfiguration.CreateFleet(_fleetArranger);
        _match = new Match(fleet);
        
        _matchViewModelUpdater.Handle(new MatchStartedEvent(_matchConfiguration));
    }

    public MatchCockpitViewModel GetMatchCockpit()
    {
        if (_matchViewModel is null)
        {
            throw new InvalidOperationException("Match has to be started first");
        }
        
        return _matchViewModel.Cockpit;
    }

    public void ShootATarget(string gridCoordinates)
    {
        if (_match is null || _matchViewModelUpdater is null)
        {
            throw new InvalidOperationException("Match has to be started first");
        }
        
        var coordinates = GridCoordinates.From(gridCoordinates).ToFleetCoords();

        if (coordinates.X >= _matchConfiguration.Constrains.Width ||
            coordinates.Y >= _matchConfiguration.Constrains.Height ||
            coordinates.X < 0||
            coordinates.Y <0)
        {
            throw new ArgumentException("Provided coords are out of grid constraints");
        }
        
        var result = _match.Handle(new ShootATarget(coordinates));
        if (result.IsFailure)
        {
            throw new Exception(result.Error);
        }
        
        foreach (var matchEvent in result.Value)
        {
            _matchViewModelUpdater.Handle(matchEvent);
        }
    }
    
    public string GetGameState()
    {
        if (_matchViewModel is null)
        {
            throw new InvalidOperationException("Match has to be started first");
        }
        return _matchViewModel.State;
    }
}