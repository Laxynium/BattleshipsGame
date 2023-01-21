using Battleships.Console.Fleets;
using Battleships.Console.SoloMode;
using FluentAssertions;
using static Battleships.UnitTests.Builders.FleetShipBuilder;

namespace Battleships.UnitTests.SoloMode;

public class GameStatesChangesTests
{
    [Fact]
    public void player_turn_state_changes_to_player_turn_state_when_there_are_still_not_sunk_ships()
    {
        var fleet = Fleet.Create(CreateShip((0, 0), (0, 1)));
        
        var playerTurnState = new PlayerTurnState(fleet);

        var state = playerTurnState.HandleChange(new TakeAShotAt((0, 0)));
        state.Should().BeOfType<PlayerTurnState>();
    }
    
    [Fact]
    public void player_turn_state_changes_to_game_finished_state_when_all_ships_got_sunk()
    {
        var fleet = Fleet.Create(CreateShip((0, 0)));
        
        var playerTurnState = new PlayerTurnState(fleet);
        
        var nextState = playerTurnState.HandleChange(new TakeAShotAt((0, 0)));

        nextState.Should().BeOfType<GameOverState>();
    }
    
    [Fact]
    public void sinking_all_the_ships_will_change_state_to_game_over()
    {
        var fleet = Fleet.Create(CreateShip((0, 0)), 
            CreateShip((1, 1)),
            CreateShip((3, 3)));
        
        IGameState state = new PlayerTurnState(fleet);
        
        state = state.HandleChange(new TakeAShotAt((0, 0)));
        state.Should().BeOfType<PlayerTurnState>();

        state = state.HandleChange(new TakeAShotAt((1, 1)));
        state.Should().BeOfType<PlayerTurnState>();
        
        state = state.HandleChange(new TakeAShotAt((3, 3)));
        state.Should().BeOfType<GameOverState>();
    }
    
    [Fact]
    public void game_over_state_cannot_be_escaped()
    {
        var fleet = Fleet.Create(CreateShip((0, 0)), 
            CreateShip((1, 1)),
            CreateShip((3, 3)));
        
        IGameState state = new GameOverState();

        state = state.HandleChange(new TakeAShotAt((0, 0)));
        state.Should().BeOfType<GameOverState>();
        
        state = state.HandleChange(new TakeAShotAt((2, 2)));
        state.Should().BeOfType<GameOverState>();
    }
}