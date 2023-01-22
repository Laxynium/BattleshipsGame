using Battleships.Console.Fleets;

namespace Battleships.Console.Matches;

public interface IMatchEvent { }
public sealed record ShootMissedEvent(Coordinates Coordinates) : IMatchEvent;
public sealed record ShootHitShipEvent(Coordinates Coordinates, FleetShipId FleetShipId) : IMatchEvent;
public sealed record ShootSunkShipEvent(Coordinates Coordinates, FleetShipId FleetShipId) : IMatchEvent;
public sealed record ShootSunkFleetEvent(Coordinates Coordinates, FleetShipId FleetShipId) : IMatchEvent;