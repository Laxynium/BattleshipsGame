using CSharpFunctionalExtensions;

namespace Battleships.Console.Application.MatchCockpit;

public class ShotResultDto : EnumValueObject<ShotResultDto>
{
    public static readonly ShotResultDto SunkFleet = new("sunk_fleet");
    public static readonly ShotResultDto SunkShip = new("sunk_ship");
    public static readonly ShotResultDto Hit = new("hit");
    public static readonly ShotResultDto Miss = new("miss");

    private ShotResultDto(string id) : base(id)
    {
    }
}
public record ShotLog(string Coordinates, ShotResultDto ShotResult, string? ShipId);