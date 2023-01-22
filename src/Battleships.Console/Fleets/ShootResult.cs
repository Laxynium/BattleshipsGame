namespace Battleships.Console.Fleets;

public abstract record ShootResult
{
    private ShootResult(){}
    public record FleetSunk(FleetShipId FleetShipId) : ShootResult;
    public record Sunk(FleetShipId FleetShipId) : ShootResult;
    public record Hit(FleetShipId FleetShipId) : ShootResult;
    public record Miss : ShootResult;

    public static FleetSunk AFleetSunk(FleetShipId fleetShipId) => new(fleetShipId);
    public static Sunk ASunk(FleetShipId fleetShipId) => new(fleetShipId);
    public static Hit AHit(FleetShipId fleetShipId) => new(fleetShipId);
    public static Miss AMiss() => new();
}