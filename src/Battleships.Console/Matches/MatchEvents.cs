using Battleships.Console.Fleets;

namespace Battleships.Console.Matches;

public interface IMatchEvent { }
public sealed record ShotMissedEvent(Coordinates Coordinates) : IMatchEvent;
public sealed record ShotHitShipEvent(Coordinates Coordinates, FleetShipId FleetShipId) : IMatchEvent;
public sealed record ShotSunkShipEvent(Coordinates Coordinates, FleetShipId FleetShipId) : IMatchEvent;
public sealed record ShotSunkFleetEvent(Coordinates Coordinates, FleetShipId FleetShipId) : IMatchEvent;