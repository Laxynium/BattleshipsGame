namespace Battleships.Console.Application.Matches;

public interface IMatchEventHandler<in TEvent>
    where TEvent: MatchEvent
{
    void Handle(TEvent @event);
}