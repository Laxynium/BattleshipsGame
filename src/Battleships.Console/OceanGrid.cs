using CSharpFunctionalExtensions;

namespace Battleships.Console;

public record Ship;

public class OceanGrid
{
    public int Width { get; }
    public int Height { get; }
    
    private OceanGrid(int width, int height)
    {
        Width = width;
        Height = height;
    }

    public static Result<OceanGrid> Create(int width, int height, params Ship[] ship)
    {
        return new OceanGrid(width, height);
    }
}