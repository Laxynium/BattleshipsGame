using OneOf;

namespace Battleships.Console.ConsoleUI;

public static class MatchAction
{
    public record StopMatch;

    public record ShotTarget(string? Coords);

    public static OneOf<ShotTarget, StopMatch> ParseMatchInput(string? input) =>
        input switch
        {
            "stop" => new StopMatch(),
            _ => new ShotTarget(input)
        };
}