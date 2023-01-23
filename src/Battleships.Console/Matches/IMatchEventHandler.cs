namespace Battleships.Console.Matches;

public interface IMatchEventHandler<in TEvent>
    where TEvent: MatchEvent
{
    void Handle(TEvent @event);
}