namespace Dungeon.Internal;

public class ScreenManager
{
    Renderer renderer = new Renderer("Dungeon Craller");
    List<ScreenInterface> screenInterfaces = new List<ScreenInterface>();
    ScreenInterface currentScreen;
    int screen = 0;
    public ScreenManager(List<ScreenInterface> screens)
    {
        currentScreen = screens[screen];
    }
    public void register(ScreenInterface screenInterface)
    {
        screenInterfaces.Add(screenInterface);
    }
    public void Run()
    {
        while (true)
        {
            Console.Clear();
            ConsoleKeyInfo input = Console.ReadKey(true);
            if (Console.KeyAvailable)
            {
                currentScreen.Input(input);
            }
            currentScreen.Update(renderer);
        }
    }
}