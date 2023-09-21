using Dungeon.Core;
using Dungeon.Geneartion;
using Dungeon.Utils;
namespace Dungeon.Screen;

public class GameScreen : IScreen
{
    private bool _update = true;
    private bool _mapReady = false;
    private Player player = new Player(1, 5);
    private World world = new World();
    public Game Context { get; set; }
    public bool NeedsInit { get; set; } = true;

    private Vector2 _worldOffset = new Vector2(0, 0);

    private Tile[] _currentMap = new Tile[0];

    public GameScreen(Game game)
    {
        Context = game;

    }

    private void GenerateLevel()
    {
        _currentMap = world.GenearteWorld();
        world.GenearteLevelName();

        _worldOffset.X = world.CurrentStartingPoint.X;
        _worldOffset.Y = world.CurrentStartingPoint.Y + 10;
    }

    public void Init()
    {
        if (!_mapReady)
        {
            GenerateLevel();
            _mapReady = true;
        }

        Context.Renderer.WriteBuffer($"Name: {player.Name}", 1, 1);
        Context.Renderer.WriteBuffer("Health: ", 1, 2);
        Context.Renderer.WriteBuffer($"World: {world.LevelName}", 1, 3);
        Context.Renderer.WriteBuffer("Gold: ", 17, 1);
    }

    public void Input(ConsoleKeyInfo input)
    {
        if (input.Key == ConsoleKey.DownArrow)
        {
            if (GetTile(player.X - 1, player.Y - 5 + 1) == Tile.Floor)
            {
                _update = player.MoveDown();
                if (player.Y >= 21 && (_worldOffset.Y + 19) < world.Height)
                {
                    player.MoveUp();
                    _worldOffset.Y += 2;
                    _update = true;
                }
            }
        }

        if (input.Key == ConsoleKey.UpArrow)
        {
            if (GetTile(player.X - 1, player.Y - 5 - 1) == Tile.Floor)
            {
                _update = player.MoveUp();
                if (player.Y <= 8 && (_worldOffset.Y - 1) >= 0)
                {
                    player.MoveDown();
                    _worldOffset.Y -= 2;
                    _update = true;
                }
            }
        }

        if (input.Key == ConsoleKey.RightArrow)
        {
            if (GetTile(player.X - 1 + 1, player.Y - 5) == Tile.Floor)
            {
                _update = player.MoveRight();
                if (player.X >= 48 && (_worldOffset.X + 50) < world.Width)
                {
                    _worldOffset.X += 2;
                    player.MoveLeft();
                    _update = true;
                }
            }
        }

        if (input.Key == ConsoleKey.LeftArrow)
        {
            if (GetTile(player.X - 1 - 1, player.Y - 5) == Tile.Floor)
            {
                _update = player.MoveLeft();
                if (player.X <= 2 && (_worldOffset.X - 1) >= 0)
                {
                    player.MoveRight();
                    _worldOffset.X -= 2;
                    _update = true;
                }
            }
        }

        if (input.Key == ConsoleKey.Escape)
        {
            Context.SetScreen(Screen.Pause);
        }
    }

    private Tile GetTile(int x, int y)
    {
        return _currentMap[_worldOffset.X + x + world.Width * (_worldOffset.Y + y)];
    }
    public void Render()
    {
        if (player.StatsAreDirty)
        {
            Context.Renderer.Write(player.Health.ToString(), 9, 2);
            Context.Renderer.Write(player.Gold.ToString(), 23, 1);
            player.StatsAreDirty = false;
        }

        if (_update)
        {
            for (int y = 0; y < 19; y++)
            {
                for (int x = 0; x < 50; x++)
                {
                    Context.Renderer.Write(GetTile(x, y), 1 + x, 5 + y);
                }
            }


            Context.Renderer.Write('X', player.X, player.Y);
            _update = false;
        }
    }
}