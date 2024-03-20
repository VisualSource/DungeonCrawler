namespace Dungeon.Core;

public class ScreenManager
{
    Renderer renderer;
    List<ScreenInterface> screenInterfaces = new List<ScreenInterface>();
    ScreenInterface currentScreen;
    int screen = 0;
    public ScreenManager(string name, List<ScreenInterface> screens)
    {
        renderer = new Renderer(name);
        screenInterfaces = screens;

        foreach (var screen in screenInterfaces) screen.Init(renderer);

        currentScreen = screenInterfaces[screen];
    }
    public void register(ScreenInterface screenInterface)
    {
        screenInterface.Init(renderer);
        screenInterfaces.Add(screenInterface);
    }
    public void Run()
    {
        Console.Clear();
        while (true)
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo input = Console.ReadKey(true);
                currentScreen.Input(input, renderer);
            }
            currentScreen.Update(renderer);
        }
    }
}