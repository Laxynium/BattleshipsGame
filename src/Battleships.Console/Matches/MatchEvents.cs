using Battleships.Console.Fleets;
using Battleships.Console.MatchConfigurations;

namespace Battleships.Console.Matches;

public interface IMatchEvent { }
public sealed record ShotMissedEvent(Coordinates Coordinates) : IMatchEvent;
public sealed record ShotHitShipEvent(Coordinates Coordinates, FleetShipId FleetShipId) : IMatchEvent;
public sealed record ShotSunkShipEvent(Coordinates Coordinates, FleetShipId FleetShipId) : IMatchEvent;
public sealed record ShotSunkFleetEvent(Coordinates Coordinates, FleetShipId FleetShipId) : IMatchEvent;
public sealed record MatchStartedEvent(MatchConfiguration MatchConfiguration) : IMatchEvent;
public sealed record MatchOverEvent : IMatchEvent;