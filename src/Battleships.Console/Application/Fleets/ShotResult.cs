namespace Battleships.Console.Application.Fleets;

public abstract record ShotResult
{
    private ShotResult(){}
    public record FleetSunk(FleetShipId FleetShipId) : ShotResult;
    public record Sunk(FleetShipId FleetShipId) : ShotResult;
    public record Hit(FleetShipId FleetShipId) : ShotResult;
    public record Miss : ShotResult;

    public static FleetSunk AFleetSunk(FleetShipId fleetShipId) => new(fleetShipId);
    public static Sunk ASunk(FleetShipId fleetShipId) => new(fleetShipId);
    public static Hit AHit(FleetShipId fleetShipId) => new(fleetShipId);
    public static Miss AMiss() => new();
}