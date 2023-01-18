namespace Battleships.Console;

public record Coordinate(int X, int Y)
{
    public static implicit operator Coordinate((int x, int y) coordinate) =>
        new(coordinate.x, coordinate.y);
}

public record FleetShip
{
    private readonly Coordinate _coordinate;

    private FleetShip(Coordinate coordinate)
    {
        _coordinate = coordinate;
    }
    
    public static FleetShip Create(Coordinate coordinate) => 
        new(coordinate);

    public Fleet.ShootResult ReceiveShot(Coordinate coordinate)
    {
        if (_coordinate == coordinate)
        {
            return Fleet.ShootResult.GotHit;
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
        GotHit, Miss
    }
}