using System.Collections.Specialized;
using Dungeon.Geneartion;
using Dungeon.Utils;

namespace Dungeon.Core;

public class World
{
    private bool _debug = false;
    public int Height = 100;
    public int Width = 150;
    public string LevelName = "LOADING";
    public Vector2 CurrentStartingPoint = new Vector2(0, 0);
    private int inRange(int value, int oldMin, int oldMax, int newMin, int newMax)
    {
        return (((value - oldMin) * (newMax - newMin)) / (oldMax - oldMin)) + newMin;
    }
    public Tuple<int, int> ToLocal(int x, int y)
    {
        return Tuple.Create(inRange(x, 0, Width, 0, 50), inRange(y, 0, Height, 0, 19));
    }
    public Tuple<int, int> ToWorld(int x, int y)
    {
        return Tuple.Create(inRange(x, 0, 50, 0, Width), inRange(y, 0, 19, 0, Height));
    }
    public void GenearteLevelName(bool debug = false)
    {
        _debug = debug;
    }
    public Tile[] GenearteWorld()
    {
        DrunkardWalk generator = new DrunkardWalk(e =>
        {
            if (_debug) Console.WriteLine(e);
        });

        CurrentStartingPoint = generator.CreateDungeon(Width, Height, 6);

        Console.Clear();
        Console.WriteLine(CurrentStartingPoint);
        Tile[] map = generator.GetMap();

        /*for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                if (y == 0 || y == (Height - 1) || x == 0 || x == Width - 1)
                {
                    Console.Write("=");
                    continue;
                }

                Console.Write((char)map[x + Width * y]);
            }

            Console.Write("\n");
        }

        Console.ReadKey();*/

        return map;
    }
}