using Battleships.Console.Application.Fleets;
using CSharpFunctionalExtensions;

namespace Battleships.Console.Application.Matches;

public class Match
{
    public string Id { get; }
    private readonly Fleet _fleet;
    private bool _matchOver;

    public Match(string id, Fleet fleet)
    {
        Id = id;
        _fleet = fleet;
        _matchOver = false;
    }
    
    public Result<IReadOnlyCollection<MatchEvent>> Handle(IMatchCommand command)
    {
        if (_matchOver)
        {
            return Result.Failure<IReadOnlyCollection<MatchEvent>>("Match has ended, cannot accept any more commands");
        }
        
        if (command is not ShootATarget shootATarget)
        {
            return Result.Success<IReadOnlyCollection<MatchEvent>>(new List<MatchEvent>());
        }

        var result = _fleet.ReceiveShot(shootATarget.Coordinates);

        var matchEvent = ToMatchEvent(result, shootATarget.Coordinates);

        if (matchEvent is ShotSunkFleetEvent)
        {
            _matchOver = true;
            return Result.Success<IReadOnlyCollection<MatchEvent>>(new List<MatchEvent>
            {
                matchEvent, new MatchOverEvent(Id)
            });
        }
        
        return Result.Success<IReadOnlyCollection<MatchEvent>>(new List<MatchEvent>
        {
            matchEvent
        });
    }

    private MatchEvent ToMatchEvent(ShotResult shotResult, Coordinates coordinates) =>
        shotResult switch
        {
            ShotResult.FleetSunk (var shipId) => new ShotSunkFleetEvent(Id, coordinates, shipId),
            ShotResult.Sunk (var shipId) => new ShotSunkShipEvent(Id, coordinates, shipId),
            ShotResult.Hit (var shipId) => new ShotHitShipEvent(Id, coordinates, shipId),
            ShotResult.Miss => new ShotMissedEvent(Id, coordinates),
            _ => throw new ArgumentOutOfRangeException(nameof(shotResult), shotResult, null)
        };
}