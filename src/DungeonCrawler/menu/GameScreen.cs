using Dungeon.Core;
using Dungeon.Geneartion;
using Dungeon.Utils;
namespace Dungeon.Screen;

public class GameScreen : IScreen
{
    private readonly Vector2 _tileOffset = new Vector2(1, 5);
    private bool _update = true;
    private bool _mapReady = false;
    private Player player = new Player(1, 5);
    private World world = new World();
    public Game Context { get; set; }
    public bool NeedsInit { get; set; } = true;

    private int _scrollSize = 2;

    private int _screenWidth = 50;
    private int _screenHeight = 19;
    private int _screenHeightHalf { get => _screenHeight / 2; }
    private int _screenWidthHalf { get => _screenWidth / 2; }

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

        _worldOffset.X = world.CurrentStartingPoint.X - _screenWidthHalf;
        _worldOffset.Y = world.CurrentStartingPoint.Y - _screenHeightHalf;

        player.X = _screenWidthHalf + _tileOffset.X;
        player.Y = _screenHeightHalf + _tileOffset.Y;
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
            if (GetTile(player.X - _tileOffset.X, player.Y - _tileOffset.Y + 1) == Tile.Floor)
            {
                _update = player.MoveDown();
                if (player.Y >= 21 && (_worldOffset.Y + _screenHeight) < world.Height)
                {
                    player.MoveUp(_scrollSize);
                    _worldOffset.Y += _scrollSize;
                    _update = true;
                }
            }
        }

        if (input.Key == ConsoleKey.UpArrow)
        {
            if (GetTile(player.X - _tileOffset.X, player.Y - _tileOffset.Y - 1) == Tile.Floor)
            {
                _update = player.MoveUp();
                if (player.Y <= 8 && (_worldOffset.Y - 1) >= 0)
                {
                    player.MoveDown(_scrollSize);
                    _worldOffset.Y -= _scrollSize;
                    _update = true;
                }
            }
        }

        if (input.Key == ConsoleKey.RightArrow)
        {
            if (GetTile(player.X - _tileOffset.X + 1, player.Y - _tileOffset.Y) == Tile.Floor)
            {
                _update = player.MoveRight();
                if (player.X >= 48 && (_worldOffset.X + 50) < world.Width)
                {
                    _worldOffset.X += _scrollSize;
                    player.MoveLeft(_scrollSize);
                    _update = true;
                }
            }
        }

        if (input.Key == ConsoleKey.LeftArrow)
        {
            if (GetTile(player.X - _tileOffset.X - 1, player.Y - _tileOffset.Y) == Tile.Floor)
            {
                _update = player.MoveLeft();
                if (player.X <= 2 && (_worldOffset.X - 1) >= 0)
                {
                    player.MoveRight(_scrollSize);
                    _worldOffset.X -= _scrollSize;
                    _update = true;
                }
            }
        }

        if (input.Key == ConsoleKey.Escape)
        {
            Context.SetScreen(Screen.Pause);
        }

        if (input.Key == ConsoleKey.Enter)
        {

            if (GetTile(player.X - _tileOffset.X, player.Y - _tileOffset.Y + 1) == Tile.Door)
            {
                //Down
                SetTile(player.X - _tileOffset.X, player.Y - _tileOffset.Y + 1, Tile.Floor);
                _update = true;
            }
            if (GetTile(player.X - _tileOffset.X, player.Y - _tileOffset.Y - 1) == Tile.Door)
            {
                //Up
                SetTile(player.X - _tileOffset.X, player.Y - _tileOffset.Y - 1, Tile.Floor);
                _update = true;
            }
            if (GetTile(player.X - _tileOffset.X + 1, player.Y - _tileOffset.Y) == Tile.Door)
            {
                //left
                SetTile(player.X - _tileOffset.X + 1, player.Y - _tileOffset.Y, Tile.Floor);
                _update = true;
            }
            if (GetTile(player.X - _tileOffset.X - 1, player.Y - _tileOffset.Y) == Tile.Door)
            {
                SetTile(player.X - _tileOffset.X - 1, player.Y - _tileOffset.Y, Tile.Floor);
                _update = true;
            }

        }

    }

    private Tile GetTile(int x, int y)
    {
        return _currentMap[_worldOffset.X + x + world.Width * (_worldOffset.Y + y)];
    }
    private void SetTile(int x, int y, Tile cell)
    {
        _currentMap[_worldOffset.X + x + world.Width * (_worldOffset.Y + y)] = cell;
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
            for (int y = 0; y < _screenHeight; y++)
            {
                for (int x = 0; x < _screenWidth; x++)
                {
                    if ((x >= player.X - 4) && (x <= player.X + 4) && (y >= player.Y - 4) && (y <= player.Y + 4))
                    {
                        Context.Renderer.Write(GetTile(x, y), _tileOffset.X + x, _tileOffset.Y + y);
                    }
                }
            }


            Context.Renderer.Write('X', player.X, player.Y);
            _update = false;
        }
    }
}