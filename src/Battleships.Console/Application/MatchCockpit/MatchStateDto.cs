using CSharpFunctionalExtensions;

namespace Battleships.Console.Application.MatchCockpit;

public class MatchStateDto : EnumValueObject<MatchStateDto>
{
    public static readonly MatchStateDto WaitingForMatch = new("waiting_for_match");
    public static readonly MatchStateDto PlayerTurn = new("player_turn");
    public static readonly MatchStateDto MatchOver = new("match_over");

    private MatchStateDto(string id) : base(id)
    {
    }
}