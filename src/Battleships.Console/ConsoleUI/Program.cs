using System.Text;
using Battleships.Console.Application;
using Battleships.Console.Application.Fleets;
using Battleships.Console.Application.MatchCockpit;
using Battleships.Console.Application.MatchConfigurations;
using CSharpFunctionalExtensions;
using Figgle;

namespace Battleships.Console.ConsoleUI;

public class Program
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
        var screenRenderer = new StringBuilder(1000);

        StringBuilder Render(params string[] lines) => RenderUsing(screenRenderer, lines);
        void Display() => DisplayUsing(screenRenderer);
        void RenderMatchCockpit(string? lastError) => RenderMatchCockpitUsing(Render, gameFacade, lastError);

        while (true)
        {
            Render(
                FiggleFonts.Standard.Render("Battleships Game"),
                "Type 'start' to start",
                "Type 'exit' to exit a game"
            );
            Display();
            
            var menuInput = ReadGameMenuInput();
            if (menuInput is null)
            {
                continue;
            }

            if (menuInput == "exit")
            {
                break;
            }

            Render("Starting a match.",
                "Type 'stop' if you want to stop playing.",
                "Press any key to begin a match...");
            Display();
            System.Console.ReadKey();

            gameFacade.StartANewMatch();
            var gameState = gameFacade.GetGameState();
            string? lastError = null;

            while (gameState != MatchStateDto.MatchOver)
            {
                RenderMatchCockpit(lastError);
                Display();

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

            if (gameState == MatchStateDto.MatchOver)
            {
                Render(FiggleFonts.Standard.Render("Congratulations!"),
                    FiggleFonts.Banner.Render("You have won!"),
                    "Press any key to continue...");
                Display();
                System.Console.ReadKey();
            }
        }

        Render("See you back soon!");
        Display();
    }

    private static StringBuilder RenderUsing(StringBuilder renderer, params string[] lines)
    {
        renderer.Clear();
        foreach (var line in lines)
        {
            renderer = renderer.AppendLine(line);
        }
        return renderer;
    }

    private static void DisplayUsing(StringBuilder renderer)
    {
        System.Console.Clear();
        System.Console.Write(renderer);
    }
    
    private static void RenderMatchCockpitUsing(Func<string[], StringBuilder> render, 
        GameFacade gameFacade,
        string? lastError)
    {
        var cockpit = gameFacade.GetMatchCockpit()!;
        var logs = cockpit.Logs.Take(3)
            .Select(MapShotLogToString)
            .ToArray();
        var error = lastError is not null ? new[] { "", lastError } : Array.Empty<string>();
        var lines = new[] { "" }
            .Concat(cockpit.TargetGrid.ToTextRepresentation())
            .Concat(new []{""})
            .Concat(logs)
            .Concat(error)
            .Concat(new[] { "" })
            .ToArray();
        
        render(lines);
    }

    private static string MapShotLogToString(ShotLog shotLog) =>
        shotLog.ShotResult switch
        {
            { } dto when dto == ShotResultDto.Miss => $"Shot at {shotLog.Coordinates} was a miss",
            { } dto when dto == ShotResultDto.Hit => $"Shot at {shotLog.Coordinates} hit a ship {shotLog.ShipId}",
            { } dto when dto == ShotResultDto.SunkShip => $"Shot at {shotLog.Coordinates} sunk a ship {shotLog.ShipId}",
            { } dto when dto == ShotResultDto.SunkFleet => $"Shot at {shotLog.Coordinates} sunk a last one fleet ship {shotLog.ShipId}",
            _ => throw new ArgumentOutOfRangeException()
        };

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