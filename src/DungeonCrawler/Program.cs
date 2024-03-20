using Dungeon.Core;
using Dungeon.Screen;

ScreenManager manager = new ScreenManager("Dungeon Crawller", new List<ScreenInterface>() { new Game() });

manager.Run();