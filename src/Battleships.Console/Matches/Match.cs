using Battleships.Console.Fleets;
using CSharpFunctionalExtensions;

namespace Battleships.Console.Matches;

public interface IMatchEvent
{
}

public interface IMatchCommand
{
}

public sealed record ShootATarget(Coordinates Coordinates) : IMatchCommand;

public class Match
{
    public Result<IReadOnlyCollection<IMatchEvent>> Handle(IMatchCommand command)
    {
        return Result.Success<IReadOnlyCollection<IMatchEvent>>(new List<IMatchEvent>());
    }
}