using Dungeon.Core;
using Dungeon.Utils;

namespace Dungeon.Screen;

public class MainMenuScreen : IScreen
{
    public bool NeedsInit { get; set; } = true;
    public Game Context { get; set; }

    private int _option = 1;

    private Vector2 _option_1_pos = new Vector2(0, 0);
    private Vector2 _option_2_pos = new Vector2(0, 0);
    public MainMenuScreen(Game game)
    {
        Context = game;
    }
    public void Init()
    {
        // Header
        Context.Renderer.WriteBuffer("Welcome", 1, 1);

        // body

        string Option1 = "Play";
        string Option2 = "Settings";

        _option_1_pos.X = (Context.Renderer.ScreenWidth / 2) - (Option1.Length / 2);
        _option_1_pos.Y = 10;

        _option_2_pos.X = (Context.Renderer.ScreenWidth / 2) - (Option2.Length / 2);
        _option_2_pos.Y = 12;

        Context.Renderer.WriteBuffer(Option1, _option_1_pos.X, _option_1_pos.Y);
        Context.Renderer.WriteBuffer(Option2, _option_2_pos.X, _option_2_pos.Y);
    }

    public void Input(ConsoleKeyInfo input)
    {
        if (input.Key == ConsoleKey.UpArrow)
        {
            if (_option > 1)
            {
                _option--;
            }
        }

        if (input.Key == ConsoleKey.DownArrow)
        {
            if (_option < 2)
            {
                _option++;
            }
        }

        if (input.Key == ConsoleKey.Enter)
        {
            Context.SetScreen((Screen)_option);
        }
    }

    public void Render()
    {
        Context.Renderer.Write($"Option {_option}", 1, 2);

        if (_option == 1)
        {
            Context.Renderer.ColorWrite("{BG=DarkGray}{FG=Gray}Play", _option_1_pos.X, _option_1_pos.Y);
            Context.Renderer.Write("Settings", _option_2_pos.X, _option_2_pos.Y);
        }

        if (_option == 2)
        {
            Context.Renderer.Write("Play", _option_1_pos.X, _option_1_pos.Y);
            Context.Renderer.ColorWrite("{BG=DarkGray}{FG=Gray}Settings", _option_2_pos.X, _option_2_pos.Y);
        }
    }
}