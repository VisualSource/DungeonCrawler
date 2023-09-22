using Dungeon.Core;

namespace Dungeon.Screen;

public class ErrorScreen : IScreen
{
    public bool NeedsInit { get; set; } = true;
    public Game Context { get; set; }

    private string _errorMessage = "Unkown Error";

    public ErrorScreen(Game game)
    {
        Context = game;
    }

    public void SetErrorMessage(string value)
    {
        _errorMessage = value;
    }
    public void Init()
    {

    }

    public void Input(ConsoleKeyInfo input)
    {
        if (input.Key == ConsoleKey.Enter)
        {

        }
    }

    public void Render()
    {
        Context.Renderer.ColorWrite($"{_errorMessage}", (Context.Renderer.ScreenWidth / 2) - 2, (Context.Renderer.ScreenWidth / 2));
        Context.Renderer.ColorWrite("{FG=Gray}{BG=DarkGray}Ok", (Context.Renderer.ScreenWidth / 2) - 2, (Context.Renderer.ScreenWidth / 2) + 5);
    }
}