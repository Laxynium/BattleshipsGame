using System.Text;
using Battleships.Console.Application.MatchCockpit;
using Figgle;

namespace Battleships.Console.ConsoleUI;

public class Screen
{
    private readonly StringBuilder _screen = new(1000);

    public void DisplayGameWelcomeScreen()
    {
        _screen.Clear();
        AppendLines(FiggleFonts.Standard.Render("Battleships Game"),
            "Type 'start' to start",
            "Type 'exit' to exit a game");
        Render();
    }

    public void DisplayStartingAMatchInformation()
    {
        _screen.Clear();
        AppendLines(
            "You are about to start a match!",
            "Type 'stop' if you want to stop playing.",
            "Press any key to begin a match...");
        Render();
    }
    
    public void UpdateMatchCockpit(MatchCockpitViewModel matchCockpit, string? lastError)
    {
        var logs = matchCockpit.Logs.Take(3)
            .Select(MapShotLogToString)
            .ToArray();
        var error = lastError is not null ? new[] { "", lastError } : Array.Empty<string>();
        var lines = new[] { "" }
            .Concat(matchCockpit.TargetGrid.ToTextRepresentation())
            .Concat(new []{""})
            .Concat(logs)
            .Concat(error)
            .Concat(new[] { "" })
            .ToArray();

        _screen.Clear();
        AppendLines(lines);
    }

    public void DisplayMessageToTypeCoords()
    {
        AppendLines("Type your target coordinates: ");
        Render();
    }

    public void DisplayWinScreen()
    {
        _screen.Clear();
        AppendLines(FiggleFonts.Standard.Render("Congratulations!"),
            FiggleFonts.Banner.Render("You have won!"),
            "Press any key to continue...");
        Render();
    }

    public void DisplayGoodbyeScreen()
    {
        _screen.Clear();
        AppendLines(FiggleFonts.Banner.Render("See you back soon!"));
        Render();
    }

    private static string MapShotLogToString(ShotLog shotLog) =>
        shotLog.ShotResult switch
        {
            { } dto when dto == ShotResultDto.Miss => $"Shot at {shotLog.Coordinates} was a miss",
            { } dto when dto == ShotResultDto.Hit => $"Shot at {shotLog.Coordinates} hit a {shotLog.ShipName}({shotLog.ShipId})",
            { } dto when dto == ShotResultDto.SunkShip => $"Shot at {shotLog.Coordinates} sunk a {shotLog.ShipName}({shotLog.ShipId})",
            { } dto when dto == ShotResultDto.SunkFleet => $"Shot at {shotLog.Coordinates} sunk a last one fleet ship which was {shotLog.ShipName}({shotLog.ShipId})",
            _ => throw new ArgumentOutOfRangeException()
        };
    
    private void AppendLines(params string[] lines)
    {
        foreach (var line in lines)
        {
            _screen.AppendLine(line);
        }
    }

    private void Render()
    {
        System.Console.Clear();
        System.Console.Write(_screen);
    }
    
}