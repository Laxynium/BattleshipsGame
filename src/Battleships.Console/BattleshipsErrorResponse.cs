namespace Battleships.Console;

public class BattleshipsErrorResponse
{
    public string Reason { get; }

    public BattleshipsErrorResponse(string reason)
    {
        Reason = reason;
    }
}