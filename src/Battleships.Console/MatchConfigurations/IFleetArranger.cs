using Battleships.Console.Fleets;

namespace Battleships.Console.MatchConfigurations;

public interface IFleetArranger
{
    IReadOnlyCollection<(FleetShipId shipId, CoordinatesSet coords)> GetShipsArrangement(MatchConfiguration matchConfiguration);
}