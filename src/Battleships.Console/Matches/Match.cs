using Battleships.Console.Fleets;
using CSharpFunctionalExtensions;

namespace Battleships.Console.Matches;

public interface IMatchEvent
{
}

public sealed record ShootMissedEvent(Coordinates Coordinates) : IMatchEvent;
public sealed record ShootHitShipEvent(Coordinates Coordinates) : IMatchEvent;
public sealed record ShootSunkShipEvent(Coordinates Coordinates) : IMatchEvent;
public sealed record ShootSunkFleetEvent(Coordinates Coordinates) : IMatchEvent;

public interface IMatchCommand
{
}

public sealed record ShootATarget(Coordinates Coordinates) : IMatchCommand;

public class Match
{
    private readonly Fleet _fleet;
    private bool _gameOver;

    public Match(Fleet fleet)
    {
        _fleet = fleet;
    }

    public Result<IReadOnlyCollection<IMatchEvent>> Handle(IMatchCommand command)
    {
        if (_gameOver)
        {
            return Result.Failure<IReadOnlyCollection<IMatchEvent>>("Match has ended, cannot accept any more commands");
        }
        
        if (command is not ShootATarget shootATarget)
            return Result.Success<IReadOnlyCollection<IMatchEvent>>(new List<IMatchEvent>());
        
        var result = _fleet.ReceiveShot(shootATarget.Coordinates);

        var matchEvent = ToMatchEvent(result, shootATarget.Coordinates);

        if (matchEvent is ShootSunkFleetEvent)
        {
            _gameOver = true;
        }
        
        return Result.Success<IReadOnlyCollection<IMatchEvent>>(new List<IMatchEvent>
        {
            matchEvent
        });
    }

    private IMatchEvent ToMatchEvent(ShootResult shootResult, Coordinates coordinates) =>
        shootResult switch
        {
            ShootResult.FleetSunk => new ShootSunkFleetEvent(coordinates),
            ShootResult.Sunk => new ShootSunkShipEvent(coordinates),
            ShootResult.Hit => new ShootHitShipEvent(coordinates),
            ShootResult.Miss => new ShootMissedEvent(coordinates),
            _ => throw new ArgumentOutOfRangeException(nameof(shootResult), shootResult, null)
        };
}