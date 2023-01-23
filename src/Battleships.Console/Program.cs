using Battleships.Console.Fleets;
using Battleships.Console.MatchCockpit;
using Battleships.Console.MatchConfigurations;
using CSharpFunctionalExtensions;

namespace Battleships.Console;

public partial class Program
{
    private static readonly MatchConfiguration MatchConfiguration = new(
        new GridConstrains(10, 10), ShipBlueprintsStock.Create(
            ("1", ShipBlueprint.FromText("-----")),
            ("2", ShipBlueprint.FromText("----")),
            ("3", ShipBlueprint.FromText("----"))));

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

        while (true)
        {
            System.Console.WriteLine("Type 'start' to start");
            System.Console.WriteLine("Type 'exit' to exit a game");

            var menuInput = ReadGameMenuInput();
            if (menuInput is null)
            {
                continue;
            }

            if (menuInput == "exit")
            {
                break;
            }

            System.Console.WriteLine("Starting a match. Type 'stop' if you want to stop playing.");
            
            gameFacade.StartANewMatch();
            var gameState = gameFacade.GetGameState();

            while (gameState != MatchStateDto.MatchOver)
            {
                ShowMatchCockpit(gameFacade);

                var userInput = ReadMatchUserInput();
                if (userInput is MatchUserInput.StopMatch)
                {
                    break;
                }

                if (userInput is MatchUserInput.TargetCoords coords)
                {
                    var result = gameFacade.ShootATarget(coords.Value!);
                    if (result.IsFailure)
                    {
                        System.Console.WriteLine(result.Error.Reason);
                    }
                }

                gameState = gameFacade.GetGameState();
            }

            if (gameState == MatchStateDto.MatchOver)
            {
                System.Console.WriteLine("Congratulations! You have won a battleship match");    
            }
        }

        System.Console.WriteLine("See you back soon!");
    }

    private static void ShowMatchCockpit(GameFacade gameFacade)
    {
        System.Console.WriteLine();
        var cockpit = gameFacade.GetMatchCockpit()!;
        foreach (var gridLine in cockpit.TargetGrid.ToTextRepresentation())
        {
            System.Console.WriteLine(gridLine);
        }
        System.Console.WriteLine();
    }

    private static string? ReadGameMenuInput()
    {
        var input = System.Console.ReadLine();
        return input switch
        {
            "start" => "start",
            "exit" => "exit",
            _ => null
        };
    }
    
    private static MatchUserInput ReadMatchUserInput()
    {
        System.Console.Write("Type your target coordinates: ");
        var input = System.Console.ReadLine();

        Func<string?, Maybe<MatchUserInput>>[] pipe = new[] { MatchUserInput.StopMatch.Parse, MatchUserInput.TargetCoords.Parse };

        var result = pipe.Aggregate(Maybe<MatchUserInput>.None, 
            (acc, x) => acc.Or(x(input)));

        return result.GetValueOrDefault(new MatchUserInput.TargetCoords(null));
    }

    private abstract record MatchUserInput
    {
        public record StopMatch : MatchUserInput
        {
            public static Maybe<MatchUserInput> Parse(string? input)
            {
                if (input == "stop")
                {
                    return Maybe.From<MatchUserInput>(new StopMatch());
                }
                return Maybe<MatchUserInput>.None;
            }
        }

        public record TargetCoords(string? Value) : MatchUserInput
        {
            public static Maybe<MatchUserInput> Parse(string? input)
            {
                return Maybe.From<MatchUserInput>(new TargetCoords(input));
            }
        }
    }
}