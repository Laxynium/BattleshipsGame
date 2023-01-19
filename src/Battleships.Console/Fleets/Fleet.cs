namespace Battleships.Console.Fleets;

public record Fleet
{
    private readonly List<FleetShip> _ships;

    private Fleet(IEnumerable<FleetShip> ships) => 
        _ships = ships.ToList();

    public static Fleet Create(FleetShip ship, params FleetShip[] ships)
    {
        var allShips = new[] { ship }.Concat(ships).ToList();

        var areThereOverlappingShips = CartesianProduct(allShips)
            .Where(c => c.x != c.y)
            .Any(c => c.x.IsOverlappingWith(c.y));

        if (areThereOverlappingShips)
        {
            throw new ShipsAreOverlappingException();
        }
        
        return new(allShips);
    }

    public ShootResult ReceiveShot(Coordinate coordinate)
    {
        foreach (var ship in _ships)
        {
            var result = ship.ReceiveShot(coordinate);
            if (result == ShootResult.Hit)
            {
                return ShootResult.Hit;
            }
        }

        return ShootResult.Miss;
    }


    private static IEnumerable<(T x, T y)> CartesianProduct<T>(IList<T> items) => 
        from x in items 
        from y in items 
        select (x, y);
}