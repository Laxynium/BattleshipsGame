using Battleships.Console.Fleets;
using FluentAssertions;

namespace Battleships.UnitTests.SoloMode;

public class GameStatesChangesTests
{
    [Fact]
    public void player_turn_state_changes_to_player_turn_state_when_there_are_still_not_sunk_ships()
    {
        var fleet = Fleet.Create(FleetShip.Create((0, 0), (0, 1)));
        
        var playerTurnState = new PlayerTurnState(fleet);

        var state = playerTurnState.HandleChange(new TakeAShotAt((0, 0)));
        state.Should().BeOfType<PlayerTurnState>();
    }
    
    [Fact(Skip = "Need to first extend fleets capabilities")]
    public void player_turn_state_changes_to_game_finished_state_when_all_ships_got_sunk()
    {
        var fleet = Fleet.Create(FleetShip.Create((0, 0)));
        
        var playerTurnState = new PlayerTurnState(fleet);
        
        var nextState = playerTurnState.HandleChange(new TakeAShotAt((0, 0)));

        nextState.Should().BeOfType<GameOverState>();
    }
}

public record TakeAShotAt(Coordinate Coordinate);

public interface IGameState
{
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
        return new PlayerTurnState(_fleet);
    }
}

public class GameOverState : IGameState
{
}