namespace Battleships.Console.MatchCockpit;

public enum Cell{None, WhitePeg, RedPeg}

public record TargetGrid(Cell[][] Cells)
{
    public static TargetGrid FromTextRepresentation(IEnumerable<string> lines)
    {
        var cells = lines.Skip(1)
            .Select(x => x.Split(" ").Skip(1).Select(ToCell).ToArray())
            .ToArray();
        return new TargetGrid(cells);
    }

    private static Cell ToCell(string symbol) =>
        symbol switch
        {
            "!" => Cell.RedPeg,
            "@" => Cell.WhitePeg,
            "_" => Cell.None,
            _ => throw new ArgumentOutOfRangeException(nameof(symbol), symbol, null)
        };


    public string[] ToTextRepresentation()
    {
        var rowsCount = Cells.Length;
        var columnsCount = Cells[0].Length;

        var firstLine = GetFirstLine(columnsCount);

        var lines = new[] { firstLine }
            .Concat(Enumerable.Range(0, rowsCount)
                .Select(row => GetRowText(row, columnsCount)));
        return lines.ToArray();
    }

    private static string GetFirstLine(int columnsCount) =>
        string.Join(" ",
            new[] { "x" }.Concat(Enumerable.Range(0, columnsCount).Select(column => (column + 1).ToString())));

    private string GetRowText(int row, int columnsCount) =>
        string.Join(" ", RowCoordinate(row).Concat(Enumerable
            .Range(0, columnsCount)
            .Select(column => ToText(Cells[row][column]))));

    private static IEnumerable<string> RowCoordinate(int row) => new[] { ((char)('A' + row)).ToString() };

    private string ToText(Cell cell) =>
        cell switch
        {
            Cell.RedPeg => "!",
            Cell.WhitePeg => "@",
            Cell.None => "_",
            _ => throw new ArgumentOutOfRangeException(nameof(cell), cell, null)
        };
};