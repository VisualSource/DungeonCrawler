using Dungeon.Core;

namespace Dungeon.Screen;

public class PauseScreen : IScreen
{
    public bool NeedsInit { get; set; } = true;
    public Game Context { get; set; }

    public PauseScreen(Game game)
    {
        Context = game;
    }
    public void Init()
    {
        Context.Renderer.WriteBuffer("Pause", 1, 1);
    }

    public void Input(ConsoleKeyInfo input)
    {
        if (input.Key == ConsoleKey.Escape)
        {
            Context.SetScreen(Screen.Game);
        }

    }

    public void Render()
    {

    }
}