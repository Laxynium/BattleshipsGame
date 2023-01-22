using Battleships.Console.Matches;

namespace Battleships.Console;

public interface IMatchEventHandler<in TEvent>
    where TEvent: IMatchEvent
{
    void Handle(TEvent @event);
}