using System.Text;
using Dungeon.Core;

namespace Dungeon.Screen;

public class Game : ScreenInterface
{
    private static int HEADER_OFFSET = 5;
    private bool dirty = true;
    private bool headerDirty = true;
    private List<List<char>> buffer = new List<List<char>>();
    private List<List<char>> header = new List<List<char>>();
    public Game() { }
    public void Init(Renderer renderer)
    {
        StringBuilder sb = new StringBuilder();
        sb.Insert(0, "═", renderer.ScreenWidth);
        sb[0] = '╔';
        sb[renderer.ScreenWidth - 1] = '╗';

        header.Add(sb.ToString().ToList());

        StringBuilder k = new StringBuilder();
        k.Insert(0, " ", renderer.ScreenWidth);
        k[0] = '║';
        k[renderer.ScreenWidth - 1] = '║';

        for (var i = 0; i < HEADER_OFFSET - 2; i++)
        {
            header.Add(k.ToString().ToList());
        }
        sb[0] = '╠';
        sb[renderer.ScreenWidth - 1] = '╣';

        header.Add(sb.ToString().ToList());

        k[0] = '║';
        k[renderer.ScreenWidth - 1] = '║';

        for (var i = 0; i < renderer.ScreenHeight - HEADER_OFFSET - 1; i++)
        {
            buffer.Add(k.ToString().ToList());
        }

        sb[0] = '╚';
        sb[renderer.ScreenWidth - 1] = '╝';

        buffer.Add(sb.ToString().ToList());
    }

    public void Input(ConsoleKeyInfo input, Renderer renderer)
    {
        if (input.Key == ConsoleKey.UpArrow)
        {
            renderer.Write(2, renderer.ScreenWidth / 2, "{FG=Green}Hello{/FG}");

            headerDirty = true;
        }
    }

    public void Update(Renderer renderer)
    {
        if (headerDirty)
        {
            renderer.Write(0, 0, header);
            headerDirty = false;
        }
        if (dirty)
        {
            renderer.Write(0, HEADER_OFFSET, buffer);
            dirty = false;
        }
    }
}