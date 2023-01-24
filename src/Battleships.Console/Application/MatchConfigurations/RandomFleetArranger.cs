using Battleships.Console.Application.Fleets;

namespace Battleships.Console.Application.MatchConfigurations;

public class RandomFleetArranger : IFleetArranger
{
    private readonly Random _random = new();
    public IReadOnlyCollection<(FleetShipId shipId, CoordinatesSet coords)> GetShipsArrangement(MatchConfiguration matchConfiguration)
    {
        var ships = matchConfiguration.BlueprintsStock.ShipBlueprints
            .Select(x => x.shipBlueprint.Set)
            .ToArray();

        var shipsArrangement = ships.Select(s =>
                ArrangementsGenerator.GenerateFor(s, matchConfiguration.Constrains))
            .ToArray();
        
        var pickedShipsArrangement = ShipsArrangementPicker.PickShipsArrangement(ships, RandomShipArrangementsPicker,
            i => shipsArrangement[i]);

        return pickedShipsArrangement.Select((a, i) => (matchConfiguration.BlueprintsStock.ShipBlueprints[i].id, a))
            .ToList();
    }

    private CoordinatesSet RandomShipArrangementsPicker(IReadOnlyList<CoordinatesSet> shipArrangements)
    {
        var index = _random.Next(0, shipArrangements.Count - 1);
        return shipArrangements[index];
    }
}