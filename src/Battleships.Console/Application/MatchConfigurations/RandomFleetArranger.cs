using Battleships.Console.Application.Fleets;

namespace Battleships.Console.Application.MatchConfigurations;

public class RandomFleetArranger : IFleetArranger
{
    public IReadOnlyCollection<(FleetShipId shipId, CoordinatesSet coords)> GetShipsArrangement(MatchConfiguration matchConfiguration)
    {
        return new List<(FleetShipId id, CoordinatesSet coords)>();
    }
}