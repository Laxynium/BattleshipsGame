namespace Battleships.Console.Application;

public class BattleshipsErrorResponse
{
    public string Reason { get; }

    public BattleshipsErrorResponse(string reason)
    {
        Reason = reason;
    }
}