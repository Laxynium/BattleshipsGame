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
        
        if (fleetArrangement.Select(x => x.shipId.Value).ToArray()
                .Intersect(_blueprintsStock.ShipBlueprints.Select(x => x.id.Value).ToArray())
                .Count() != _blueprintsStock.ShipBlueprints.Count)
        {
            throw new ArgumentException("Fleet arrangement does not match what was specified in ship blueprints stock");
        }
        
        var ships = fleetArrangement.Select(x=>FleetShip.Create("1", x.coords))
            .ToArray();
        
        return Fleet.Create(ships[0], ships.Skip(1).ToArray());
    }
}