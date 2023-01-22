using Battleships.Console.Fleets;
using CSharpFunctionalExtensions;

namespace Battleships.Console.Matches;

public class Match
{
    private readonly Fleet _fleet;
    private bool _matchOver;

    public Match(Fleet fleet)
    {
        _fleet = fleet;
    }
    
    public Result<IReadOnlyCollection<IMatchEvent>> Handle(IMatchCommand command)
    {
        if (_matchOver)
        {
            return Result.Failure<IReadOnlyCollection<IMatchEvent>>("Match has ended, cannot accept any more commands");
        }
        
        if (command is not ShootATarget shootATarget)
            return Result.Success<IReadOnlyCollection<IMatchEvent>>(new List<IMatchEvent>());
        
        var result = _fleet.ReceiveShot(shootATarget.Coordinates);

        var matchEvent = ToMatchEvent(result, shootATarget.Coordinates);

        if (matchEvent is ShotSunkFleetEvent)
        {
            _matchOver = true;
            return Result.Success<IReadOnlyCollection<IMatchEvent>>(new List<IMatchEvent>
            {
                matchEvent, new MatchOverEvent()
            });
        }
        
        return Result.Success<IReadOnlyCollection<IMatchEvent>>(new List<IMatchEvent>
        {
            matchEvent
        });
    }

    private static IMatchEvent ToMatchEvent(ShotResult shotResult, Coordinates coordinates) =>
        shotResult switch
        {
            ShotResult.FleetSunk (var id) => new ShotSunkFleetEvent(coordinates, id),
            ShotResult.Sunk (var id) => new ShotSunkShipEvent(coordinates, id),
            ShotResult.Hit (var id) => new ShotHitShipEvent(coordinates, id),
            ShotResult.Miss => new ShotMissedEvent(coordinates),
            _ => throw new ArgumentOutOfRangeException(nameof(shotResult), shotResult, null)
        };
}