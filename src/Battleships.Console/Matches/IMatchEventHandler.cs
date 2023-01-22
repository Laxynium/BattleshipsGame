namespace Battleships.Console.Matches;

public interface IMatchEventHandler<in TEvent>
    where TEvent: IMatchEvent
{
    void Handle(TEvent @event);
}