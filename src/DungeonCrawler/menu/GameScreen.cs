using Dungeon.Core;
using Dungeon.Geneartion;
using Dungeon.Utils;
namespace Dungeon.Screen;

public class GameScreen : IScreen
{
    public bool NeedsInit { get; set; } = true;
    public Game Context { get; set; }
    private readonly Vector2 _tileOffset = new Vector2(1, 5);
    private bool _update = true;
    private Player player = new Player(1, 5);
    private int _scrollSize = 2;
    private int _mapWidth = 150;
    private int _mapHeight = 100;
    private int _screenWidth = 50;
    private int _screenHeight = 19;
    private int _screenHeightHalf { get => _screenHeight / 2; }
    private int _screenWidthHalf { get => _screenWidth / 2; }

    private Tuple<string, Vector2>? _entranceLadder = null;

    private Vector2 _worldOffset = new Vector2(0, 0);

    private Level? _level;

    public GameScreen(Game game)
    {
        Context = game;

    }
    private void GenerateLevel()
    {
        if (_entranceLadder is not null)
        {
            _level = Level.LoadLevel("../content/", _entranceLadder.Item1);
            _entranceLadder = null;
        }

        if (_level is null)
        {
            DrunkardWalk generator = new DrunkardWalk(e => { });
            _level = generator.CreateDungeon(_mapWidth, _mapHeight, 6, _entranceLadder);
        }

        _worldOffset.X = _level.StartPoint.X - _screenWidthHalf;
        _worldOffset.Y = _level.StartPoint.Y - _screenHeightHalf;

        player.X = _screenWidthHalf + _tileOffset.X;
        player.Y = _screenHeightHalf + _tileOffset.Y;
    }

    public void Init()
    {
        if (_level is null)
        {
            GenerateLevel();
        }

        if (_level is null) throw new Exception("No level set");

        Context.Renderer.WriteBuffer($"Name: {player.Name}", 1, 1);
        Context.Renderer.WriteBuffer("Health: ", 1, 2);
        Context.Renderer.WriteBuffer($"World: {_level.Name}", 1, 3);
        Context.Renderer.WriteBuffer("Gold: ", 17, 1);
    }

    public void Input(ConsoleKeyInfo input)
    {
        if (input.Key == ConsoleKey.DownArrow)
        {
            if (GetTile(player.X - _tileOffset.X, player.Y - _tileOffset.Y + 1) == Tile.Floor)
            {
                _update = player.MoveDown();
                if (player.Y >= 21 && (_worldOffset.Y + _screenHeight) < _mapHeight)
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
                if (player.X >= 48 && (_worldOffset.X + 50) < _mapWidth)
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
        if (_level is null) throw new Exception("No Level Set");
        return _level.Map[_worldOffset.X + x + _mapWidth * (_worldOffset.Y + y)];
    }
    private void SetTile(int x, int y, Tile cell)
    {
        if (_level is null) throw new Exception("No Level Set");
        _level.Map[_worldOffset.X + x + _mapWidth * (_worldOffset.Y + y)] = cell;
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
                    if ((y >= player.Y - _tileOffset.Y - 2) && (y <= player.Y - _tileOffset.Y + 2) && (x >= player.X - _tileOffset.X - 4) && (x <= player.X - _tileOffset.X + 4))
                    {
                        Context.Renderer.Write(GetTile(x, y), _tileOffset.X + x, _tileOffset.Y + y);
                    }
                    else
                    {
                        Context.Renderer.Write(Tile.Unused, _tileOffset.X + x, _tileOffset.Y + y);
                    }
                }
            }

            Context.Renderer.Write('X', player.X, player.Y);
            _update = false;
        }
    }
}