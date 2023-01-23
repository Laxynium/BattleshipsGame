using Battleships.Console.Application.MatchCockpit;
using FluentAssertions;

namespace Battleships.UnitTests.MatchCockpit;

public class TargetGridTextRepresentationTests
{
    [Fact]
    public void target_grid_can_be_constructed_from_text_representation()
    {
        var targetGrid = TargetGrid.FromTextRepresentation(new[]
        {
            "x 1 2 3",
            "A ! ! @",
            "B _ ! @",
            "C _ @ _",
        });

        targetGrid.Cells.Should().BeEquivalentTo(new[]
        {
            new[] { Cell.RedPeg, Cell.RedPeg, Cell.WhitePeg },
            new[] { Cell.None, Cell.RedPeg, Cell.WhitePeg },
            new[] { Cell.None, Cell.WhitePeg, Cell.None },
        });
    }
    
    [Fact]
    public void target_grid_can_be_converted_to_text_representation()
    {
        var targetGrid = new TargetGrid(new[]
        {
            new[] { Cell.RedPeg, Cell.RedPeg, Cell.WhitePeg },
            new[] { Cell.None, Cell.RedPeg, Cell.WhitePeg },
            new[] { Cell.None, Cell.WhitePeg, Cell.None },
        });

        var textRepresentation = targetGrid.ToTextRepresentation();

        textRepresentation.Should().BeEquivalentTo(new[]
        {
            "x 1 2 3",
            "A ! ! @",
            "B _ ! @",
            "C _ @ _",
        });
    }
}