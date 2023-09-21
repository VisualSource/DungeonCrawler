using System.Text.RegularExpressions;
using Dungeon.Geneartion;
using Microsoft.VisualBasic;

namespace Dungeon.Core;
public class Renderer
{
    private List<List<char>> buffer = new List<List<char>> { };

    public bool IsDirty { get; set; } = true;

    public int ScreenWidth = 52;
    public int ScreenHeight = 25;
    public Renderer(string title)
    {
        Console.CursorVisible = false;
        Console.Title = title;
        Console.OutputEncoding = System.Text.Encoding.Unicode;

        // Border
        Reset();
    }

    public void Reset()
    {
        buffer = new List<List<char>>();

        for (int y = 0; y < ScreenHeight; y++)
        {
            var row = new List<char> { };

            for (int x = 0; x < ScreenWidth; x++)
            {
                char item = GetBorderChar(x, y);
                row.Add(item);
            }
            buffer.Add(row);
        }
    }

    public void Write(Tile content, int left, int top)
    {
        Console.SetCursorPosition(left, top);
        Console.Write((char)content);
    }

    public void Write(char content, int left, int top)
    {
        Console.SetCursorPosition(left, top);
        Console.Write(content);
    }
    public void Write(string content, int left, int top)
    {
        Console.SetCursorPosition(left, top);
        Console.Write(content);
    }
    public void WriteDebug(string content)
    {
        Console.SetCursorPosition(0, 25);
        Console.Write("                                                     ");
        Console.SetCursorPosition(0, 25);
        Console.Write("DEBUG: {0}", content);
    }

    public void WriteBuffer(Tile content, int left, int top)
    {
        buffer[top][left] = (char)content;
        IsDirty = true;
    }
    public void WriteBuffer(char content, int left, int top)
    {
        buffer[top][left] = content;
        IsDirty = true;
    }
    public void WriteBuffer(string content, int left, int top)
    {
        if (content.Length > 52) throw new Exception($"Content length is larger then viewport. Length({content.Length})");


        for (int i = 0; i < content.Length; i++)
        {
            buffer[top][left + i] = content[i];
        }

        IsDirty = true;
    }
    /// <summary>
    /// https://stackoverflow.com/questions/54122982/how-to-color-words-in-different-colours-in-a-console-writeline-in-a-console-appl
    /// </summary>
    /// <param name="content"></param>
    /// <param name="left"></param>
    /// <param name="top"></param>
    public void ColorWrite(string content, int left, int top)
    {
        Console.SetCursorPosition(left, top);

        var initialBackgroundColor = Console.BackgroundColor;
        var initialForegroundColor = Console.ForegroundColor;

        var colorMatch = Regex.Match(content, @"{[FB]G=[a-z]+}|{\/[FB]G}", RegexOptions.IgnoreCase);

        var current_index = 0;
        while (colorMatch.Success)
        {
            if ((colorMatch.Index - current_index) > 0)
            {
                Console.Write(content.Substring(current_index, colorMatch.Index - current_index));
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

        Console.Write(content.Substring(current_index));

        Console.BackgroundColor = initialBackgroundColor;
        Console.ForegroundColor = initialForegroundColor;
    }

    public void WriteVer(string content, int left, int top)
    {
        if (top - 1 > 0)
        {
            Console.SetCursorPosition(0, top - 1);
            Console.Write(new string(buffer[top - 1].ToArray()));
        }

        WriteHor(content, left, top);

        if (top + 1 < 51)
        {
            Console.SetCursorPosition(0, top + 1);
            Console.Write(new string(buffer[top + 1].ToArray()));
        }
    }
    public void WriteHor(string content, int left, int top)
    {
        string line = "";

        for (int i = 0; i < buffer[top].Count; i++)
        {
            if (i >= left && i < left + content.Length)
            {

                line += content[i - left];

                continue;
            }

            line += buffer[top][i];
        }

        Console.SetCursorPosition(0, top);
        Console.Write(line);
    }
    /// <summary>
    /// https://en.wikipedia.org/wiki/Box-drawing_character
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private char GetBorderChar(int x, int y)
    {
        if (x == 0 && y == 0)
        {
            return '╔';
        }
        if (x == 51 && y == 0)
        {
            return '╗';
        }
        if (x == 0 && y == 4)
        {
            return '╠';
        }
        if (x == 51 && y == 4)
        {
            return '╣';
        }
        if (x == 0 && y == 24)
        {
            return '╚';
        }
        if (x == 51 && y == 24)
        {
            return '╝';
        }
        if (x == 0 || x == 51)
        {
            return '║';
        }

        if (x >= 1 && x <= 51 && y == 4 || y == 0 || y == 24)
        {
            return '=';
        }

        return ' ';
    }
    public void Render()
    {
        Console.Clear();

        foreach (var col in buffer)
        {
            foreach (var row in col)
            {
                Console.Write(row);
            }
            Console.Write("\n");
        }

        IsDirty = false;
    }
}