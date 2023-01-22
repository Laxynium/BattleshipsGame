using Battleships.Console.Fleets;
using Battleships.Console.MatchCockpit;

namespace Battleships.Console.MatchConfigurations;

public interface IFleetArranger
{
    IReadOnlyCollection<(FleetShipId shipId, CoordinatesSet coords)> GetShipsArrangement(MatchConfiguration matchConfiguration);
}

public class FixedFleetArranger : IFleetArranger
{
    private readonly List<(FleetShipId shipId, CoordinatesSet coords)> _fixedPlacement;

    public FixedFleetArranger(IEnumerable<(FleetShipId shipId, string[] gridCoords)> fixedPlacement)
    {
        _fixedPlacement = fixedPlacement.Select(x =>
            {
                var coords = x.gridCoords
                    .Select(CoordinatesTranslator.AFleetCoordinates)
                    .ToArray();
                return (x.shipId,xyz: CoordinatesSet.Create(coords[0], coords.Skip(1).ToArray()));
            })
            .ToList();
    }

    public IReadOnlyCollection<(FleetShipId shipId, CoordinatesSet coords)> GetShipsArrangement(MatchConfiguration matchConfiguration) => 
        _fixedPlacement;
}

public class RandomFleetArranger : IFleetArranger
{
    public IReadOnlyCollection<(FleetShipId shipId, CoordinatesSet coords)> GetShipsArrangement(MatchConfiguration matchConfiguration)
    {
        return new List<(FleetShipId id, CoordinatesSet coords)>();
    }
}