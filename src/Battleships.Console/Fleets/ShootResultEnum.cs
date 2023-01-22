namespace Battleships.Console.Fleets;

public abstract record ShootResult
{
    private ShootResult(){}
    public record FleetSunk(FleetShipId FleetShipId) : ShootResult;
    public record Sunk(FleetShipId FleetShipId) : ShootResult;
    public record Hit(FleetShipId FleetShipId) : ShootResult;
    public record Miss : ShootResult;
}

public enum ShootResultEnum
{
    Hit,
    Miss,
    Sunk,
    FleetSunk
}