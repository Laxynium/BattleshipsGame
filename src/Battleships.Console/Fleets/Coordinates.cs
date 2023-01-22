using CSharpFunctionalExtensions;

namespace Battleships.Console.Fleets;

public class Coordinates : ValueObject
{
    public int X { get; }
    public int Y { get; }
    
    public Coordinates(int x, int y)
    {
        Y = y;
        X = x;
    }

    public static implicit operator Coordinates((int x, int y) coordinate) =>
        new(coordinate.x, coordinate.y);

    public IEnumerable<Coordinates> GetNeighbourhood()
    {
        yield return new Coordinates(X - 1, Y);
        yield return new Coordinates(X + 1, Y);
        yield return new Coordinates(X, Y - 1);
        yield return new Coordinates(X, Y + 1);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return X;
        yield return Y;
    }

    public void Deconstruct(out int x, out int y)
    {
        x = X;
        y = Y;
    }
}