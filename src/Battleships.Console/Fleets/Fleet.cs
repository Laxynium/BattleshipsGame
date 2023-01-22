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

    public ShootResult ReceiveShot(Coordinates coordinates)
    {
        foreach (var ship in _ships)
        {
            var result = ship.ReceiveShot(coordinates);

            if (result is ShootResult.Miss)
            {
                continue;
            }

            return IsFleetSunk()
                ? new ShootResult.FleetSunk(GetFleetShipId(result))
                : result;
        }

        return new ShootResult.Miss();
    }

    private bool IsFleetSunk()
    {
        return _ships.All(x => x.IsSunk());
    }

    private static FleetShipId GetFleetShipId(ShootResult shootResult) =>
        shootResult switch
        {
            ShootResult.Hit (var id) => id,
            ShootResult.Sunk (var id) => id,
            ShootResult.FleetSunk (var id) => id,
            _ => throw new ArgumentOutOfRangeException(nameof(shootResult), shootResult, null)
        };


    private static IEnumerable<(T x, T y)> CartesianProduct<T>(IList<T> items) =>
        from x in items
        from y in items
        select (x, y);
}