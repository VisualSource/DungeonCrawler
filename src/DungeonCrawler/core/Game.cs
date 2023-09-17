namespace Dungeon.Core;

public class Game {
    private Renderer renderer = new Renderer("Dungeon crawler");
    private Player player = new Player(1,6);

    private World world = new World();
    public Game(){}

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
            if(renderer.IsDirty) {
                renderer.Render();
            }

            // Stop ReadKey from blocking
            if(Console.KeyAvailable){
                var input = Console.ReadKey(true);
                bool update = false;

                if(input.Key == ConsoleKey.DownArrow){
                    update = player.MoveDown();
                }

                if(input.Key == ConsoleKey.UpArrow){
                    update = player.MoveUp();
                }

                if(input.Key == ConsoleKey.RightArrow){
                    update = player.MoveRight();
                }

                if(input.Key == ConsoleKey.LeftArrow){
                    update = player.MoveLeft();
                }

                if(update) { 
                    renderer.WriteVer("X",player.X,player.Y); 
                }
            }

            if(player.StatsAreDirty){
                renderer.Write(player.Health.ToString(),9,2);
                renderer.Write(player.Gold.ToString(),23,1);
                player.StatsAreDirty = false;
            }
        }
    }
}