using System.Transactions;
using Dungeon.Screen;

namespace Dungeon.Core;

public class Game
{
    private IScreen[] _screens;
    private int curIdx = 0;
    public Renderer Renderer = new Renderer("Dungeon crawler");
    public Game()
    {
        _screens = new IScreen[]{
            new MainMenuScreen(this),
            new GameScreen(this),
            new SettingsScreen(this),
            new PauseScreen(this),
            new ErrorScreen(this)
        };
    }

    public void SetError(string reason)
    {
        var screen = (ErrorScreen)_screens[(int)Screen.Screen.Error];

        screen.SetErrorMessage(reason);

        SetScreen(Screen.Screen.Error);
    }
    public void SetScreen(Screen.Screen screen)
    {
        curIdx = (int)screen;

        Renderer.Reset();

        _screens[curIdx].NeedsInit = true;
        Renderer.IsDirty = true;
    }
    public void Run()
    {
        IScreen current = _screens[curIdx];
        int prev = 0;
        while (true)
        {
            if (prev != curIdx)
            {
                current = _screens[curIdx];
                prev = curIdx;
            }

            if (current.NeedsInit)
            {
                current.Init();
                current.NeedsInit = false;
            }

            if (Renderer.IsDirty)
            {
                Renderer.Render();
            }

            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo input = Console.ReadKey(true);
                current.Input(input);
            }

            current.Render();
        }
    }
}