using Battleships.Console.Application;
using Battleships.Console.Application.Fleets;
using Battleships.Console.Application.MatchCockpit;
using Battleships.Console.Application.MatchConfigurations;
using CSharpFunctionalExtensions;

namespace Battleships.Console.ConsoleUI;

public class Program
{
    private static readonly MatchConfiguration MatchConfiguration = new(
        new GridConstrains(10, 10), ShipBlueprintsStock.Create(
            ("1", ShipBlueprint.FromText("Battleship", "-----")),
            ("2", ShipBlueprint.FromText("Destroyer", "----")),
            ("3", ShipBlueprint.FromText("Destroyer", "----"))));

    //TODO replace with random fleet arranger
    private static readonly FixedFleetArranger FleetArranger = new(new[]
    {
        (new FleetShipId("1"), new GridCoordinates[] { "A1", "A2", "A3", "A4", "A5" }),
        (new FleetShipId("2"), new GridCoordinates[] { "B1", "B2", "B3", "B4" }),
        (new FleetShipId("3"), new GridCoordinates[] { "C1", "C2", "C3", "C4" }),
    });

    private static void Main()
    {
        var gameFacade = new GameFacade(MatchConfiguration, FleetArranger);
        var screen = new Screen();

        while (true)
        {
            screen.DisplayGameWelcomeScreen();

            var menuInput = ReadGameMenuInput();
            if (menuInput is null)
            {
                continue;
            }

            if (menuInput == "exit")
            {
                break;
            }

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

        screen.DisplayGoodbyeScreen();
    }

    private static MatchStateDto PlayAMatch(GameFacade gameFacade, Screen screen)
    {
        var gameState = gameFacade.GetGameState();
        string? lastError = null;

        while (gameState != MatchStateDto.MatchOver)
        {
            var matchCockpit = gameFacade.GetMatchCockpit()!;

            screen.DisplayMatchCockpit(matchCockpit, lastError);

            var userInput = ReadMatchUserInput();
            if (userInput is MatchUserInput.StopMatch)
            {
                break;
            }

            if (userInput is MatchUserInput.TargetCoords coords)
            {
                var result = gameFacade.ShootATarget(coords.Value!);
                lastError = result.IsFailure ? result.Error.Reason : null;
            }

            gameState = gameFacade.GetGameState();
        }

        return gameState;
    }

    private static string? ReadGameMenuInput() =>
        System.Console.ReadLine() switch
        {
            "start" => "start",
            "exit" => "exit",
            _ => null
        };

    private static MatchUserInput ReadMatchUserInput()
    {
        System.Console.Write("Type your target coordinates: ");
        var input = System.Console.ReadLine();

        Func<string?, Maybe<MatchUserInput>>[] pipe =
            { MatchUserInput.StopMatch.Parse, MatchUserInput.TargetCoords.Parse };

        var result = pipe.Aggregate(Maybe<MatchUserInput>.None,
            (acc, x) => acc.Or(x(input)));

        return result.GetValueOrDefault(new MatchUserInput.TargetCoords(null));
    }

    private abstract record MatchUserInput
    {
        public record StopMatch : MatchUserInput
        {
            public static Maybe<MatchUserInput> Parse(string? input) =>
                input == "stop"
                    ? Maybe.From<MatchUserInput>(new StopMatch())
                    : Maybe<MatchUserInput>.None;
        }

        public record TargetCoords(string? Value) : MatchUserInput
        {
            public static Maybe<MatchUserInput> Parse(string? input) =>
                Maybe.From<MatchUserInput>(new TargetCoords(input));
        }
    }
}