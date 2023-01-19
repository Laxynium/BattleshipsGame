﻿namespace Battleships.Console;

public record Coordinate(int X, int Y)
{
    public static implicit operator Coordinate((int x, int y) coordinate) =>
        new(coordinate.x, coordinate.y);
}

public record FleetShip
{
    private readonly HashSet<Coordinate> _coordinates;

    private FleetShip(IEnumerable<Coordinate> coordinates) => 
        _coordinates = coordinates.ToHashSet();

    public static FleetShip Create(Coordinate coordinate, params Coordinate[] coordinates) => 
        new(new []{coordinate}.Concat(coordinates));

    public Fleet.ShootResult ReceiveShot(Coordinate coordinate) => 
        _coordinates.Contains(coordinate) ? Fleet.ShootResult.Hit : Fleet.ShootResult.Miss;

    public bool OverlapsWith(FleetShip another) => 
        _coordinates.Overlaps(another._coordinates);
}

public record Fleet
{
    private readonly List<FleetShip> _ships;

    private Fleet(IEnumerable<FleetShip> ships) => 
        _ships = ships.ToList();

    public static Fleet Create(FleetShip ship, params FleetShip[] ships)
    {
        var allShips = new[] { ship }.Concat(ships).ToList();
        
        var areThereOverlappingShips = allShips
            .SelectMany(s => allShips.Where(s2 => s2 != s).Select(s2 => (s, s2)))
            .Any(x => x.s.OverlapsWith(x.s2));

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
        

    public enum ShootResult
    {
        Hit, Miss
    }
}

public class ShipsAreOverlappingException : Exception
{
}