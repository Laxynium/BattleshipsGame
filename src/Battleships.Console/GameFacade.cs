using Battleships.Console.MatchCockpit;
using Battleships.Console.MatchConfigurations;
using Battleships.Console.Matches;

namespace Battleships.Console;

public class GameFacade
{
    private readonly MatchConfiguration _matchConfiguration;
    private readonly IFleetArranger _fleetArranger;

    private Match? _match;
    private MatchCockpitViewModel? _cockpitViewModel;
    private MatchCockpitUpdater? _matchCockpitUpdater;
    
    public GameFacade(MatchConfiguration matchConfiguration, IFleetArranger fleetArranger)
    {
        _matchConfiguration = matchConfiguration;
        _fleetArranger = fleetArranger;
    }
    
    public void StartANewMatch()
    {
        var fleet = _matchConfiguration.CreateFleet(_fleetArranger);
        _match = new Match(fleet);
        _cockpitViewModel = EmptyMatchCockpit();
        _matchCockpitUpdater = new MatchCockpitUpdater(_cockpitViewModel);
    }

    public MatchCockpitViewModel GetMatchCockpit()
    {
        if (_cockpitViewModel is null)
        {
            throw new InvalidOperationException("New match has to be started first");
        }
        
        return _cockpitViewModel;
    }

    public void ShootATarget(string gridCoordinates)
    {
        if (_match is null || _matchCockpitUpdater is null)
        {
            throw new InvalidOperationException("New match has to be started first");
        }
        
        var coordinates = CoordinatesTranslator.AFleetCoordinates(gridCoordinates);

        var result = _match.Handle(new ShootATarget(coordinates));
        if (result.IsFailure)
        {
            throw new Exception(result.Error);
        }
        
        foreach (var matchEvent in result.Value)
        {
            _matchCockpitUpdater.Handle(matchEvent);
        }
    }

    private static MatchCockpitViewModel EmptyMatchCockpit()
    {
        return new MatchCockpitViewModel(new TargetGrid(
                Enumerable.Range(0, 10).Select(y => 
                        Enumerable.Range(0, 10).Select(x => Cell.None).ToArray())
                    .ToArray()),
            new List<ShotLog>());
    }
}