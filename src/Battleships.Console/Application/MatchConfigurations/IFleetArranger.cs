using Battleships.Console.Application.Fleets;

namespace Battleships.Console.Application.MatchConfigurations;

public interface IFleetArranger
{
    IReadOnlyCollection<(FleetShipId shipId, CoordinatesSet coords)> GetShipsArrangement(MatchConfiguration matchConfiguration);
}