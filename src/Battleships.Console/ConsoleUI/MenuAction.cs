using OneOf;

namespace Battleships.Console.ConsoleUI;

public static class MenuAction
{
    public record Exit;

    public record StartMatch;

    public record Invalid;

    public static OneOf<StartMatch, Exit, Invalid> ParseMenuInput(string? input) =>
        input switch
        {
            "start" => new StartMatch(),
            "exit" => new Exit(),
            _ => new Invalid()
        };
}