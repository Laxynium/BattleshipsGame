using Battleships.Console.Fleets;

namespace Battleships.Console.SoloMode;

public record TakeAShotAt(Coordinate Coordinate);

public interface IGameState
{
    IGameState HandleChange(TakeAShotAt takeAShotAt);
}

public class PlayerTurnState : IGameState
{
    private readonly Fleet _fleet;

    public PlayerTurnState(Fleet fleet)
    {
        _fleet = fleet;
    }

    public IGameState HandleChange(TakeAShotAt takeAShotAt)
    {
        var result = _fleet.ReceiveShot(takeAShotAt.Coordinate);
        
        if (result == ShootResult.FleetSunk)
        {
            return new GameOverState();
        }
        
        return new PlayerTurnState(_fleet);
    }
}

public class GameOverState : IGameState
{
    public IGameState HandleChange(TakeAShotAt takeAShotAt)
    {
        return new GameOverState();
    }
}