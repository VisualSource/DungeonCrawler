using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
namespace Dungeon.Core;

public class Renderer
{
    public int ScreenWidth { init; get; }
    public int ScreenHeight { init; get; }
    public Renderer(string title, int width = 52, int height = 25)
    {
        ScreenHeight = height;
        ScreenWidth = width;
        Console.CursorVisible = false;
        Console.Title = title;
        Console.OutputEncoding = System.Text.Encoding.Unicode;
    }
    public void WriteAt(int left, int top, char value)
    {
        Console.SetCursorPosition(left, top);
        Console.Write(value);
    }
    private void WriteToScreen(int left, int top, string line)
    {
        var initialBackgroundColor = Console.BackgroundColor;
        var initialForegroundColor = Console.ForegroundColor;

        var colorMatch = Regex.Match(line, @"{[FB]G=[a-z]+}|{\/[FB]G}", RegexOptions.IgnoreCase);
        int current_index = 0;

        while (colorMatch.Success)
        {
            if ((colorMatch.Index - current_index) > 0)
            {
                Console.Write(line.Substring(current_index, colorMatch.Index - current_index));
            }
            if (colorMatch.Value.StartsWith("{BG=", StringComparison.OrdinalIgnoreCase))
            {
                var match = colorMatch.Value.Substring(4, colorMatch.Value.IndexOf("}") - 4);
                Console.BackgroundColor = Enum.TryParse<ConsoleColor>(match, true, out var parsedColor) ? parsedColor : initialBackgroundColor;
            }
            else if (colorMatch.Value.StartsWith("{FG=", StringComparison.OrdinalIgnoreCase))
            {
                var match = colorMatch.Value.Substring(4, colorMatch.Value.IndexOf("}") - 4);
                Console.ForegroundColor = Enum.TryParse<ConsoleColor>(match, true, out var parsedColor) ? parsedColor : initialForegroundColor;
            }
            else if (colorMatch.Value.Equals("{/BG}", StringComparison.OrdinalIgnoreCase))
            {
                Console.BackgroundColor = initialBackgroundColor;
            }
            else if (colorMatch.Value.Equals("{/FG}", StringComparison.OrdinalIgnoreCase))
            {
                Console.ForegroundColor = initialForegroundColor;
            }

            current_index = colorMatch.Index + colorMatch.Length;
            colorMatch = colorMatch.NextMatch();
        }

        Console.SetCursorPosition(left, top);
        Console.Write(line.Substring(current_index));

        Console.BackgroundColor = initialBackgroundColor;
        Console.ForegroundColor = initialForegroundColor;
    }

    // Supportes Color with {FG=COLOR} AND {BG=COLOR}
    public void Write(int left, int top, string value)
    {
        string[] lines = value.Split("\n");

        int start = top;
        foreach (var line in lines)
        {
            WriteToScreen(left, start, line);
            start++;
        }

    }
    public void Write(int left, int top, List<string> lines)
    {
        int start = top;
        foreach (var line in lines)
        {
            WriteToScreen(left, start, line);
            start++;
        }
    }

    public void Write(int left, int top, List<List<char>> lines)
    {
        int start = top;
        foreach (var data in lines)
        {
            var line = new string(data.ToArray());
            WriteToScreen(left, start, line);
            start++;
        }

    }
}