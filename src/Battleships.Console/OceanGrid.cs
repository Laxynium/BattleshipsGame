using CSharpFunctionalExtensions;

namespace Battleships.Console;

public record Ship;

public class OceanGrid
{
    public static Result<OceanGrid> Create(int width, int height, Ship ship)
    {
        return new OceanGrid();
    }
}