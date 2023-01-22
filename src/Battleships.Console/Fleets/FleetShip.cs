namespace Battleships.Console.Fleets;

public record FleetShip
{
    private readonly FleetShipId _id;
    private readonly Dictionary<Coordinates, bool> _holes;

    private FleetShip(FleetShipId id, IEnumerable<Coordinates> coordinates) => 
        (_id, _holes) = (id, coordinates.ToDictionary(x=>x, _=> false));

    public CoordinatesSet CoordinatesSet =>
        CoordinatesSet.Create(_holes.Keys.First(), _holes.Keys.Skip(1).ToArray());

    public static FleetShip Create(FleetShipId id, CoordinatesSet coordinatesSet)
    {
        return new FleetShip(id, coordinatesSet.Set);
    }

    public ShootResult ReceiveShot(Coordinates coordinate)
    {
        if (!_holes.ContainsKey(coordinate))
        {
            return new ShootResult.Miss();
        }

        _holes[coordinate] = true;

        return IsSunk() 
            ? new ShootResult.Sunk(_id)  
            : new ShootResult.Hit(_id);
    }

    public bool IsSunk() => 
        _holes.Values.All(x => x == true);
}