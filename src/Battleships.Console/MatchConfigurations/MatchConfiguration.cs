using Battleships.Console.Fleets;

namespace Battleships.Console.MatchConfigurations;

public record GridConstrains(int Width, int Height);

public class ShipBlueprintsStock
{
    private readonly List<(FleetShipId id, ShipBlueprint shipBlueprint)> _shipBlueprints;
    public IReadOnlyList<(FleetShipId id, ShipBlueprint shipBlueprint)> ShipBlueprints => _shipBlueprints.AsReadOnly();

    private ShipBlueprintsStock(List<(FleetShipId id, ShipBlueprint shipBlueprint)> shipBlueprints)
    {
        _shipBlueprints = shipBlueprints;
    }

    public static ShipBlueprintsStock Create((FleetShipId id, ShipBlueprint shipBlueprint) shipBlueprint,
        params (FleetShipId id, ShipBlueprint shipBlueprint)[] blueprints)
    {

        return new ShipBlueprintsStock(new[] { shipBlueprint }.Concat(blueprints).ToList());
    }
}
public class MatchConfiguration
{
    private readonly GridConstrains _gridConstrains;
    private readonly ShipBlueprintsStock _blueprintsStock;

    public MatchConfiguration(GridConstrains gridConstrains, ShipBlueprintsStock blueprintsStock)
    {
        _gridConstrains = gridConstrains;
        _blueprintsStock = blueprintsStock;
    }

    public Fleet CreateFleet(IFleetArranger fleetArranger)
    {
        var fleetArrangement = fleetArranger.GetShipsArrangement(this);

        if (fleetArrangement.Any(x => x.coords.Set.Any(c =>
                c.X < 0 || c.Y < 0 || c.X >= _gridConstrains.Width || c.Y >= _gridConstrains.Height)))
        {
            throw new ArgumentException("Fleet arrangement contains ships outside of the grid constrains");
        }

        if (CoordinatesSet.AreSomeOverlapping(fleetArrangement.Select(x => x.coords).ToArray()))
        {
            throw new ArgumentException("Fleet arrangement cannot contain overlapping ships");
        }

        if (fleetArrangement.Count != _blueprintsStock.ShipBlueprints.Count)
        {
            throw new ArgumentException("Fleet arrangement does not match what was specified in ship blueprints stock");
        }
        
        if (fleetArrangement.Select(x => x.shipId.Value).ToArray()
                .Intersect(_blueprintsStock.ShipBlueprints.Select(x => x.id.Value).ToArray())
                .Count() != _blueprintsStock.ShipBlueprints.Count)
        {
            throw new ArgumentException("Fleet arrangement does not match what was specified in ship blueprints stock");
        }

        if (fleetArrangement.Join(_blueprintsStock.ShipBlueprints, x => x.shipId, x => x.id,
                (x, y) => (x.coords.Set.Count, y.shipBlueprint.Set.Set.Count))
            .Any(x => x.Item1 != x.Item2))
        {
            throw new ArgumentException("Fleet arrangement ship size does not match one specified in blueprint");
        }
        
        var ships = fleetArrangement.Select(x=>FleetShip.Create(x.shipId, x.coords))
            .ToArray();
        
        return Fleet.Create(ships[0], ships.Skip(1).ToArray());
    }
}