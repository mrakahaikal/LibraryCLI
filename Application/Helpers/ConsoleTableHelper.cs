namespace Application.Helpers;

public enum Align
{
    Left,
    Center,
    Right
}

public readonly struct ColumnCell
{
    public string Text { get; }
    public int Width { get; }
    public Align Alignment { get; }

    public ColumnCell(string text, int width, Align alignment = Align.Left)
    {
        Text = text;
        Width = width;
        Alignment = alignment;
    }

    public string Format()
    {
        if (Text.Length >= Width)
            return Text[..Width];

        int padding = Width - Text.Length;
        return Alignment switch
        {
            Align.Left => Text.PadRight(Width),
            Align.Right => Text.PadLeft(Width),
            Align.Center => new string(' ', padding / 2) + Text + new string(' ', padding - padding / 2),
            _ => Text
        };
    }
}

public static class ConsoleTableHelper
{
    public static void PrintHeader(params ColumnCell[] columns)
    {
        Console.WriteLine(string.Join(" | ", columns.Select(c => new ColumnCell(c.Text, c.Width, Align.Center).Format())));
        Console.WriteLine(string.Join("-+-", columns.Select(c => new string('-', c.Width))));
    }

    public static void PrintRow(params ColumnCell[] columns)
    {
        Console.WriteLine(string.Join(" | ", columns.Select(c => c.Text.PadRight(c.Width))));
    }
}