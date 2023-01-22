namespace Battleships.Console.Fleets;

public record Fleet
{
    private readonly List<FleetShip> _ships;

    private Fleet(IEnumerable<FleetShip> ships) =>
        _ships = ships.ToList();

    public static Fleet Create(FleetShip ship, params FleetShip[] ships)
    {
        var allShips = new[] { ship }.Concat(ships).ToList();

        var areThereOverlappingShips = CoordinatesSet.AreSomeOverlapping(allShips
            .Select(x => x.CoordinatesSet)
            .ToArray());

        if (areThereOverlappingShips)
        {
            throw new ShipsAreOverlappingException();
        }

        return new(allShips);
    }

    public ShotResult ReceiveShot(Coordinates coordinates)
    {
        foreach (var ship in _ships)
        {
            var result = ship.ReceiveShot(coordinates);

            if (result is ShotResult.Miss)
            {
                continue;
            }

            return IsFleetSunk()
                ? new ShotResult.FleetSunk(GetFleetShipId(result))
                : result;
        }

        return new ShotResult.Miss();
    }

    private bool IsFleetSunk()
    {
        return _ships.All(x => x.IsSunk());
    }

    private static FleetShipId GetFleetShipId(ShotResult shotResult) =>
        shotResult switch
        {
            ShotResult.Hit (var id) => id,
            ShotResult.Sunk (var id) => id,
            ShotResult.FleetSunk (var id) => id,
            _ => throw new ArgumentOutOfRangeException(nameof(shotResult), shotResult, null)
        };


    private static IEnumerable<(T x, T y)> CartesianProduct<T>(IList<T> items) =>
        from x in items
        from y in items
        select (x, y);
}