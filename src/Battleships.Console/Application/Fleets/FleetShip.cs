namespace Battleships.Console.Application.Fleets;

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

    public ShotResult ReceiveShot(Coordinates coordinate)
    {
        if (!_holes.ContainsKey(coordinate))
        {
            return new ShotResult.Miss();
        }

        _holes[coordinate] = true;

        return IsSunk() 
            ? new ShotResult.Sunk(_id)  
            : new ShotResult.Hit(_id);
    }

    public bool IsSunk() => 
        _holes.Values.All(x => x == true);
}