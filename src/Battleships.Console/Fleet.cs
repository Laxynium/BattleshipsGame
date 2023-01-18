namespace Battleships.Console;

public record Coordinate(int X, int Y)
{
    public static implicit operator Coordinate((int x, int y) coordinate) =>
        new(coordinate.x, coordinate.y);
}

public record FleetShip
{
    private readonly Coordinate _coordinate;
    private readonly HashSet<Coordinate> _coordinates;

    private FleetShip(Coordinate coordinate, params Coordinate[] coordinates)
    {
        _coordinate = coordinate;
        _coordinates = coordinates.ToHashSet();
    }
    
    public static FleetShip Create(Coordinate coordinate, params Coordinate[] coordinates) => 
        new(coordinate, coordinates);

    public Fleet.ShootResult ReceiveShot(Coordinate coordinate)
    {
        if (_coordinate == coordinate)
        {
            return Fleet.ShootResult.Hit;
        }

        if (_coordinates.Contains(coordinate))
        {
            return Fleet.ShootResult.Hit;
        }

        return Fleet.ShootResult.Miss;
    }
}

public record Fleet
{
    private readonly FleetShip _ship;

    private Fleet(FleetShip ship)
    {
        _ship = ship;
    }

    public static Fleet Create(FleetShip ship) => 
        new(ship);

    public ShootResult ReceiveShot(Coordinate coordinate)
    {
        return _ship.ReceiveShot(coordinate);
        
    }

    public enum ShootResult
    {
        Hit, Miss
    }
}