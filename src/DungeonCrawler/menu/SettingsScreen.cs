using Dungeon.Core;

namespace Dungeon.Screen;

public class SettingsScreen : IScreen
{
    public bool NeedsInit { get; set; } = true;
    public Game Context { get; set; }

    public SettingsScreen(Game game)
    {
        Context = game;
    }

    public void Init()
    {
        Context.Renderer.WriteBuffer("Settings", 1, 1);
    }

    public void Input(ConsoleKeyInfo input)
    {
        if (input.Key == ConsoleKey.Backspace)
        {
            Context.SetScreen(Screen.MainMenu);
        }
    }

    public void Render()
    {

    }
}
