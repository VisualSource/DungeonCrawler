using Dungeon.Core;
namespace Dungeon.Screen;

public class GameScreen : IScreen
{
    private Player player = new Player(1, 6);
    private World world = new World();
    public Game Context { get; set; }
    public bool NeedsInit { get; set; } = true;

    public GameScreen(Game game)
    {
        Context = game;
    }
    public void Init()
    {
        Context.Renderer.WriteBuffer($"Name: {player.Name}", 1, 1);
        Context.Renderer.WriteBuffer("Health: ", 1, 2);
        Context.Renderer.WriteBuffer($"World: {world.LevelName}", 1, 3);
        Context.Renderer.WriteBuffer("Gold: ", 17, 1);

        /*var level = world.GenearteWorld();
        world.GenearteLevelName();

        for (int y = 0; y < 18; y++)
        {
            for (int x = 0; x < 48; x++)
            {
                Context.Renderer.WriteBuffer(level[(x + 20) + 50 * (y + 20)], x + 1, y + 6);
            }
        }*/
    }

    public void Input()
    {
        var input = Console.ReadKey(true);
        bool update = false;

        if (input.Key == ConsoleKey.DownArrow)
        {
            update = player.MoveDown();
        }

        if (input.Key == ConsoleKey.UpArrow)
        {
            update = player.MoveUp();
        }

        if (input.Key == ConsoleKey.RightArrow)
        {
            update = player.MoveRight();
        }

        if (input.Key == ConsoleKey.LeftArrow)
        {
            update = player.MoveLeft();
        }

        if (input.Key == ConsoleKey.E)
        {
            Context.SetScreen(0);
        }

        if (update)
        {
            Context.Renderer.WriteVer("X", player.X, player.Y);
        }
    }
    public void Render()
    {
        if (player.StatsAreDirty)
        {
            Context.Renderer.Write(player.Health.ToString(), 9, 2);
            Context.Renderer.Write(player.Gold.ToString(), 23, 1);
            player.StatsAreDirty = false;
        }
    }
}