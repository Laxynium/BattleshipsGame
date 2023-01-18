using CSharpFunctionalExtensions;

namespace Battleships.Console;

public record Ship;

public class OceanGrid
{
    public int Width { get; }
    public int Height { get; }

    private readonly List<Ship> _ships;
    public IReadOnlyList<Ship> Ships => _ships.AsReadOnly();

    private OceanGrid(int width, int height, IEnumerable<Ship> ships)
    {
        Width = width;
        Height = height;
        _ships = ships.ToList();
    }

    public static Result<OceanGrid> Create(int width, int height, params Ship[] ships)
    {
        return new OceanGrid(width, height, ships);
    }
}