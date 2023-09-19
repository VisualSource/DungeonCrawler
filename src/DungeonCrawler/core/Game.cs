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
            new TestScreen(this),
            new GameScreen(this)
        };
    }
    public void SetScreen(int screen)
    {
        if (screen > _screens.Length || screen < 0)
        {
            throw new IndexOutOfRangeException("Not Vaild screen id");
        }

        curIdx = screen;

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
                current.Input();
            }

            current.Render();
        }
    }
}