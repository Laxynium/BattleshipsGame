namespace Battleships.Console;

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