namespace Battleships.Console.Fleets;

public record Coordinates(int X, int Y)
{
    public static implicit operator Coordinates((int x, int y) coordinate) =>
        new(coordinate.x, coordinate.y);

    public IEnumerable<Coordinates> GetNeighbours()
    {
        yield return this with { X = X - 1 };
        yield return this with { X = X + 1 };
        yield return this with { Y = Y - 1 };
        yield return this with { Y = Y + 1 };
    }
}