using Dungeon.Internal;
using Dungeon.Screen;


ScreenManager manager = new ScreenManager(new List<ScreenInterface>() { new Game() });

char GetBorderChar(int x, int y)
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
        return '═';
    }

    return ' ';
}

manager.Run();

// Game loop
//while (true)
//{
//if (Console.KeyAvailable)
//{
//ConsoleKeyInfo input = Console.ReadKey(true);
/*  string? line = Console.ReadLine();
  Console.Clear();
  for (int y = 0; y < renderer.ScreenHeight; y++)
  {
      for (int x = 0; x < renderer.ScreenWidth; x++)
      {
          char item = GetBorderChar(x, y);
          renderer.WriteAt(x, y, item);
      }
  }
  Console.SetCursorPosition(0, renderer.ScreenHeight + 2);
  if (line is not null)
  {
      renderer.Write(2, 1, line);
      Console.SetCursorPosition(0, renderer.ScreenHeight + 2);
  }*/
//}
//}