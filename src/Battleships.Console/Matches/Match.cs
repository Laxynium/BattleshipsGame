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

        if (result == ShootResult.Hit)
        {
            return Result.Success<IReadOnlyCollection<IMatchEvent>>(new List<IMatchEvent>
            {
                new ShootHitShipEvent(shootATarget.Coordinates)
            });    

        }

        if (result == ShootResult.Sunk)
        {
            return Result.Success<IReadOnlyCollection<IMatchEvent>>(new List<IMatchEvent>
            {
                new ShootSunkShipEvent(shootATarget.Coordinates)
            });  
        }
            
        if (result == ShootResult.FleetSunk)
        {
            _gameOver = true;
            return Result.Success<IReadOnlyCollection<IMatchEvent>>(new List<IMatchEvent>
            {
                new ShootSunkFleetEvent(shootATarget.Coordinates)
            });  
        }

        return Result.Success<IReadOnlyCollection<IMatchEvent>>(new List<IMatchEvent>
        {
            new ShootMissedEvent((shootATarget.Coordinates))
        });

    }
}