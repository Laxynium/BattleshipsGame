using Battleships.Console.Fleets;
using Battleships.Console.MatchCockpit;
using Battleships.Console.Matches;

namespace Battleships.Console;

public class GameFacade
{
    private readonly Fleet _originalFleet;
    
    private Match? _match;
    private MatchCockpitViewModel? _cockpitViewModel;
    private MatchCockpitUpdater? _matchCockpitUpdater;

    public GameFacade(Fleet fleet)
    {
        _originalFleet = fleet;
    }

    public void StartANewMatch()
    {
        _match = new Match(_originalFleet.CreateACopy());
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