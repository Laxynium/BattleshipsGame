using Battleships.Console.Fleets;
using CSharpFunctionalExtensions;

namespace Battleships.Console;

public class GridCoordinates : ValueObject
{
    public char RowCoord { get; }
    public int ColumnCoord { get; }

    private GridCoordinates(char rowCoord, int columnCoord)
    {
        RowCoord = rowCoord;
        ColumnCoord = columnCoord;
    }

    public static Result<GridCoordinates> Parse(string? text)
    {
        if (text is null || text.Length < 2)
        {
            return Result.Failure<GridCoordinates>("Provided coordinates are in invalid format.");
        }
        
        var rowCoord = text[0];
        if (rowCoord is < 'A' or > 'Z')
        {
            return Result.Failure<GridCoordinates>("Row coordinate has to be a letter from A-Z.");
        }

        if (!int.TryParse(new string(text.Skip(1).ToArray()), out var columnCoord))
        {
            return Result.Failure<GridCoordinates>("Column coordinate has to be a number.");
        }
        
        return Result.Success(new GridCoordinates(rowCoord, columnCoord));
    }
    
    public static GridCoordinates From(string text)
    {
        var result = Parse(text);
        if (result.IsFailure)
        {
            throw new ArgumentException(result.Error);
        }

        return result.Value;
    }
    
    public static GridCoordinates From(Coordinates coordinates)
    {
        var (x, y) = coordinates;

        var (row, column) = ((char)('A' + y), x + 1);

        return new GridCoordinates(row, column);

    }

    public Coordinates ToFleetCoords()
    {
        var row = RowCoord-'A';
        var column = ColumnCoord - 1;
        return new Coordinates(column, row);
    }

    public override string ToString() => $"{RowCoord}{ColumnCoord}";

    public static implicit operator GridCoordinates(string text) => From(text);
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return RowCoord;
        yield return ColumnCoord;
    }
}