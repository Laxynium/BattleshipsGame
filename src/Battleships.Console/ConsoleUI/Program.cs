using Battleships.Console.Application;
using Battleships.Console.Application.MatchCockpit;
using Battleships.Console.Application.MatchConfigurations;

namespace Battleships.Console.ConsoleUI;

public class Program
{
    private static readonly MatchConfiguration MatchConfiguration = new(
        new GridConstrains(10, 10), ShipBlueprintsStock.Create(
            ("1", ShipBlueprint.FromText("Battleship", "-----")),
            ("2", ShipBlueprint.FromText("Destroyer", "----")),
            ("3", ShipBlueprint.FromText("Destroyer", "----"))));

    private static readonly IFleetArranger FleetArranger = new RandomFleetArranger();

    private static void Main()
    {
        var gameFacade = new GameFacade(MatchConfiguration, FleetArranger);
        var screen = new Screen();
        var breakLoop = false;

        while (!breakLoop)
        {
            screen.DisplayGameWelcomeScreen();

            var input = System.Console.ReadLine();
            MenuAction.ParseMenuInput(input)
                .Switch(
                    statMatch => StartAMatch(screen, gameFacade),
                    exit => breakLoop = true,
                    invalid => { });
        }

        screen.DisplayGoodbyeScreen();
    }

    private static void StartAMatch(Screen screen, GameFacade gameFacade)
    {
        screen.DisplayStartingAMatchInformation();

        System.Console.ReadKey();

        gameFacade.StartANewMatch();

        var gameState = PlayAMatch(gameFacade, screen);

        if (gameState == MatchStateDto.MatchOver)
        {
            screen.DisplayWinScreen();

            System.Console.ReadKey();
        }
    }

    private static MatchStateDto PlayAMatch(GameFacade gameFacade, Screen screen)
    {
        var gameState = gameFacade.GetGameState();
        string? lastError = null;
        var breakLoop = false;

        while (gameState != MatchStateDto.MatchOver && !breakLoop)
        {
            var matchCockpit = gameFacade.GetMatchCockpit()!;

            screen.UpdateMatchCockpit(matchCockpit, lastError);
            screen.DisplayMessageToTypeCoords();

            var input = System.Console.ReadLine();
            MatchAction.ParseMatchInput(input).Switch(
                shootTarget =>
                {
                    var result = gameFacade.ShootATarget(shootTarget.Coords!);
                    lastError = result.IsFailure ? result.Error.Reason : null;
                    gameState = gameFacade.GetGameState();
                },
                stop => breakLoop = true);
        }

        return gameState;
    }
}