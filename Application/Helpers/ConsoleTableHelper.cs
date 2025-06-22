namespace Application.Helpers;

public static class ConsoleTableHelper
{
    public static void PrintHeader(params (string Label, int Width)[] columns)
    {
        Console.WriteLine(string.Join(" | ", columns.Select(c => c.Label.PadRight(c.Width))));
        Console.WriteLine(string.Join("-+-", columns.Select(c => new string('-', c.Width))));
    }

    public static void PrintRow(params (string Text, int Width)[] columns)
    {
        Console.WriteLine(string.Join(" | ", columns.Select(c => c.Text.PadRight(c.Width))));
    }
}