using Battleships.Console.Fleets;

namespace Battleships.Console.MatchCockpit;

public static class CoordinatesTranslator
{
    public static Coordinates AFleetCoordinates(string gridCoordinates)
    {
        var row = gridCoordinates[0]-'A';
        var column = gridCoordinates[1]-'1';
        return new Coordinates(column, row);
    }

    public static string AGridCoordinates(Coordinates fleetCoordinates)
    {
        var (column, row) = fleetCoordinates;

        return $"{(char)('A'+row)}{(column+1).ToString()}";
    }
}