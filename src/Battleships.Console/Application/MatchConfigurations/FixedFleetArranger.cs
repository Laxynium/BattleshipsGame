using Battleships.Console.Application.Fleets;

namespace Battleships.Console.Application.MatchConfigurations;

public class FixedFleetArranger : IFleetArranger
{
    private readonly List<(FleetShipId shipId, CoordinatesSet coords)> _fixedPlacement;

    public FixedFleetArranger(IEnumerable<(FleetShipId shipId, GridCoordinates[] gridCoords)> fixedPlacement)
    {
        _fixedPlacement = fixedPlacement.Select(x =>
            {
                var coords = x.gridCoords
                    .Select(gridCoordinates => gridCoordinates.ToFleetCoords())
                    .ToArray();
                return (x.shipId,xyz: CoordinatesSet.Create(coords[0], coords.Skip(1).ToArray()));
            })
            .ToList();
    }

    public IReadOnlyCollection<(FleetShipId shipId, CoordinatesSet coords)> GetShipsArrangement(MatchConfiguration matchConfiguration) => 
        _fixedPlacement;
}