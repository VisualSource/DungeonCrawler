namespace Dungeon.Internal;

class ScreenManager
{
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
            ConsoleKeyInfo input = Console.ReadKey(true);
            if (Console.KeyAvailable)
            {
                currentScreen.Input(input);
            }
            currentScreen.Update();
        }
    }
}