using Battleships.Console.Fleets;
using CSharpFunctionalExtensions;

namespace Battleships.Console.Matches;

public interface IMatchEvent
{
}

public sealed record ShootMissedEvent(Coordinates Coordinates) : IMatchEvent;

public interface IMatchCommand
{
}

public sealed record ShootATarget(Coordinates Coordinates) : IMatchCommand;

public class Match
{
    private readonly Fleet _fleet;

    public Match(Fleet fleet)
    {
        _fleet = fleet;
    }

    public Result<IReadOnlyCollection<IMatchEvent>> Handle(IMatchCommand command)
    {
        return Result.Success<IReadOnlyCollection<IMatchEvent>>(new List<IMatchEvent>
        {
            new ShootMissedEvent((0, 0))
        });
    }
}