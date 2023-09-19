using Dungeon.Screen;

namespace Dungeon.Core;

public class Game {
    private IScreen[] _screens;

    private int curIdx = 0;

    private Renderer renderer = new Renderer("Dungeon crawler");
   

    private World world = new World();
    public Game(){
         _screens = new[]{
            new GameScreen()
        };
    }

    private void RenderHeader(){
        renderer.WriteBuffer(String.Format("Name: {0}",player.Name),1,1); 
        renderer.WriteBuffer("Health: ",1,2);
        renderer.WriteBuffer(String.Format("World: {0}",world.LevelName),1,3);
        renderer.WriteBuffer("Gold: ",17,1);


       

    }

    public void Run(){
        var level = world.GenearteWorld();
        world.GenearteLevelName();

        for (int y = 0; y < 18; y++)
        {
            for (int x = 0; x < 48; x++)
            {
                renderer.WriteBuffer(level[(x + 20) + 50 * (y + 20)],x + 1,y + 6);
            }
        }

        RenderHeader();
    
        while (true)
        {
            _screens[curIdx].Render(renderer);
            _screens[curIdx].Input(renderer);
        }
    }
}