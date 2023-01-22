namespace Battleships.Console.Fleets;

public record FleetShip
{
    private readonly Dictionary<Coordinates, bool> _holes;

    private FleetShip(IEnumerable<Coordinates> coordinates) => 
        _holes = coordinates.ToDictionary(x=>x, _=> false);

    public CoordinatesSet CoordinatesSet =>
        CoordinatesSet.Create(_holes.Keys.First(), _holes.Keys.Skip(1).ToArray());

    public static FleetShip Create(CoordinatesSet coordinatesSet)
    {
        return new FleetShip(coordinatesSet.Set);
    }

    public ShootResult ReceiveShot(Coordinates coordinate)
    {
        if (!_holes.ContainsKey(coordinate))
        {
            return new ShootResult.Miss();
        }

        _holes[coordinate] = true;

        return IsSunk() 
            ? new ShootResult.Sunk(new FleetShipId("1"))  
            : new ShootResult.Hit(new FleetShipId("1"));
    }

    public bool IsSunk() => 
        _holes.Values.All(x => x == true);
}