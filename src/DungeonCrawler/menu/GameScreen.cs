using Dungeon.Core;
namespace Dungeon.Screen;

public class GameScreen : IScreen
{
    private Player player = new Player(1,6);

    public void Input(Renderer renderer)
    {
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

    public void Render(Renderer renderer)
    {
        if(renderer.IsDirty) {
            renderer.Render();
        }
    }
}