using Battleships.Console.Fleets;
using Battleships.Console.MatchConfigurations;

namespace Battleships.Console.Matches;

public abstract record MatchEvent(string MatchId);
public sealed record ShotMissedEvent(string MatchId, Coordinates Coordinates) : MatchEvent(MatchId);
public sealed record ShotHitShipEvent(string MatchId, Coordinates Coordinates, FleetShipId FleetShipId) : MatchEvent(MatchId);
public sealed record ShotSunkShipEvent(string MatchId, Coordinates Coordinates, FleetShipId FleetShipId) : MatchEvent(MatchId);
public sealed record ShotSunkFleetEvent(string MatchId, Coordinates Coordinates, FleetShipId FleetShipId) : MatchEvent(MatchId);
public sealed record MatchStartedEvent(string MatchId, MatchConfiguration MatchConfiguration) : MatchEvent(MatchId);
public sealed record MatchOverEvent(string MatchId) : MatchEvent(MatchId);