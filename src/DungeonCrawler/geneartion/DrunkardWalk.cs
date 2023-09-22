using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Dungeon.Utils;
namespace Dungeon.Geneartion;

/// <summary>
///     https://www.roguebasin.com/index.php?title=Dungeon-Building_Algorithm
///     
///     https://www.roguebasin.com/index.php/CSharp_Example_of_a_Dungeon-Building_Algorithm
/// </summary>
public class DrunkardWalk
{
    struct BoundingBox
    {
        public Vector2 TopLeft { get; init; }
        public Vector2 BottomRight { get; init; }
        public int Width { get; init; }
        public int Heigth { get; init; }

        public override string ToString()
        {
            return $"<BoundingBox Width={Width} Height={Heigth} TopLeft={TopLeft} BottomRight={BottomRight}/>";
        }
    }
    private const int SpecialRoomChance = 10;
    private const int ChanceRoom = 70;
    public int MapMaxWidth = 200;
    public int MapMaxHeight = 200;
    private int _mapWidth;
    private int _mapHeight;
    private int _features;
    private Tile[] _dungeonMap = { };
    private readonly FakeRandom _fakeRnd = new FakeRandom();
    private readonly DefaultRandom _rnd = new DefaultRandom();
    private readonly Action<string> _logger;
    private readonly Room[] _NormalRooms;
    private readonly Room[] _StartingRooms;
    private readonly Room[] _SpecialRooms;
    public DrunkardWalk(Action<string> logger)
    {
        _logger = logger;

        _NormalRooms = LoadRoomFile("data/NormalRooms.json");
        _StartingRooms = LoadRoomFile("data/StartingRooms.json");
        _SpecialRooms = LoadRoomFile("data/SpecialRooms.json");
    }
    private Room[] LoadRoomFile(string path)
    {
        string raw = File.ReadAllText(path);
        Room[]? rooms = JsonSerializer.Deserialize<Room[]>(raw);

        if (rooms is null) throw new FileLoadException($"Failed to load file {path}");

        return rooms;
    }
    public Tile[] GetMap()
    {
        return _dungeonMap;
    }
    public Vector2 CreateDungeon(int inX, int inY, int inFeatures)
    {
        if (inFeatures < 1 || inX < 10 || inY < 10) throw new ArgumentOutOfRangeException();

        _features = inFeatures;
        _mapWidth = inX > MapMaxWidth ? MapMaxWidth : inX;
        _mapHeight = inY > MapMaxHeight ? MapMaxHeight : inY;

        _logger($"Map Width: {_mapWidth}");
        _logger($"Map Height: {_mapHeight}");
        _logger($"Map Features: {_features}");

        _dungeonMap = new Tile[_mapWidth * _mapHeight];

        for (int y = 0; y < _mapHeight; y++)
        {
            for (int x = 0; x < _mapWidth; x++)
            {
                SetCell(x, y, Tile.Unused);
            }
        }

        Direction currentDirection = RandomDirection();
        Vector2? currentExit = null;
        BoundingBox? currentFeature;
        int currentFeatures = 0;

        currentFeature = MakeRoom(_mapWidth / 2, _mapHeight / 2, RandomDirection(), 2);

        _logger($"Current Direction: {currentDirection} {currentFeature}");
        if (currentFeature is null) throw new Exception("Failed to init map");

        Vector2 startingPoint = GetStartingPoint(currentFeature.Value);

        SetCell(startingPoint.X, startingPoint.Y, Tile.Start);

        currentFeatures++;
        currentExit = GetRoomExit(currentFeature.Value, currentDirection);

        for (int countingTries = 0; countingTries < 1000; countingTries++)
        {
            if (currentFeatures == _features)
            {
                break;
            }

            if (currentExit is null) continue;

            int feature = _rnd.Next(0, 100);

            if (feature <= ChanceRoom)
            {
                int special = _rnd.Next(0, 100);

                _logger($"Current: {currentDirection} {currentFeature}");
                BoundingBox? nextFeature = MakeRoom(currentExit.X, currentExit.Y, currentDirection, special <= SpecialRoomChance ? 0 : 1);

                if (!nextFeature.HasValue)
                {
                    continue;
                    //throw new Exception("Unable to place feature");
                }

                SetDoor(currentExit, currentDirection);

                currentFeature = nextFeature;

                Direction nextDirection = GetExludedRandomDirection(new Direction[] { GetInvaildDirs(currentDirection) });

                currentExit = GetRoomExit(currentFeature.Value, nextDirection);

                currentDirection = nextDirection;

                currentFeatures++;

                //_logger($"Current: {currentDirection} {currentFeature}");

                continue;
            }
            else if (feature >= ChanceRoom)
            {
                _logger($"Current: {currentDirection} {currentFeature}");

                BoundingBox? nextFeature = MakeCorridor(currentExit.X, currentExit.Y, 4, currentDirection);

                if (nextFeature is null)
                {
                    continue;
                    //throw new Exception("Unable to place corridor");
                }

                SetDoor(currentExit, currentDirection);

                currentFeature = nextFeature;
                Direction nextDirection = GetExludedRandomDirection(new Direction[] { GetInvaildDirs(currentDirection) });

                currentExit = GetRoomExit(currentFeature.Value, nextDirection);

                currentFeatures++;

                currentDirection = nextDirection;

                _logger($"Features {currentFeatures}");
            }
        }

        return startingPoint;
    }
    private Vector2 GetStartingPoint(BoundingBox box)
    {

        int x = _rnd.Next(box.TopLeft.X + 1, box.BottomRight.X - 1);
        int y = _rnd.Next(box.TopLeft.Y + 1, box.BottomRight.Y + 1);

        return new Vector2(x, y);
    }
    private Direction GetInvaildDirs(Direction startD)
    {
        switch (startD)
        {
            case Direction.North:
                return Direction.South;
            case Direction.West:
                return Direction.East;
            case Direction.East:
                return Direction.West;
            case Direction.South:
                return Direction.North;
            default:
                return startD;
        }
    }
    private Vector2 GetRoomExit(BoundingBox bounding, Direction d)
    {
        Vector2 topLeft = bounding.TopLeft;
        Vector2 bottomRight = bounding.BottomRight;
        int width = bounding.Width;
        int height = bounding.Heigth;

        switch (d)
        {
            case Direction.North:
                {
                    for (int x = 0; x < width; x++)
                    {
                        int idx = _rnd.Next(topLeft.X + 1, bottomRight.X - 1);
                        _logger($"Exit At ({idx},{topLeft.Y}) Wall: {(char)GetCellType(idx, topLeft.Y)} Floor: {(char)GetCellType(idx, topLeft.Y + 1)}");
                        if (GetCellType(idx, topLeft.Y) != Tile.Wall || GetCellType(idx, topLeft.Y + 1) != Tile.Floor) continue;
                        return new Vector2(idx, topLeft.Y);
                    }

                    throw new ArgumentOutOfRangeException($"Unable to make a door in the given direction ({d},\n{bounding})");
                }
            case Direction.South:
                {
                    for (int x = 0; x < width; x++)
                    {
                        int idx = _rnd.Next(topLeft.X + 1, bottomRight.X - 1);
                        if (GetCellType(idx, bottomRight.Y) != Tile.Wall || GetCellType(idx, bottomRight.Y - 1) != Tile.Floor) continue;
                        _logger($"Exit At ({idx},{bottomRight.Y})");
                        return new Vector2(idx, bottomRight.Y);
                    }
                    throw new ArgumentOutOfRangeException($"Unable to make a door in the given direction ({d},\n{bounding})");
                }
            case Direction.West:
                {
                    for (int y = 0; y < height; y++)
                    {
                        int idx = _rnd.Next(topLeft.Y + 1, bottomRight.Y - 1);
                        _logger($"({topLeft.X},{idx}) Wall: {(char)GetCellType(topLeft.X, idx)} Floor: {GetCellType(topLeft.X + 1, idx)}");
                        if (GetCellType(topLeft.X, idx) != Tile.Wall || GetCellType(topLeft.X + 1, idx) != Tile.Floor) continue;
                        _logger($"Exit At ({topLeft.X},{idx})");
                        return new Vector2(topLeft.X, idx);
                    }

                    throw new ArgumentOutOfRangeException($"Unable to make a door in the given direction ({d},\n{bounding})");
                }
            case Direction.East:
                {
                    for (int y = 0; y < height; y++)
                    {
                        int idx = _rnd.Next(topLeft.Y + 1, bottomRight.Y - 1);
                        if (GetCellType(bottomRight.X, idx) != Tile.Wall || GetCellType(bottomRight.X - 1, idx) != Tile.Floor) continue;
                        _logger($"Exit At ({bottomRight.X},{idx})");
                        return new Vector2(bottomRight.X, idx);
                    }
                    throw new ArgumentOutOfRangeException($"Unable to make a door in the given direction ({d},\n{bounding})");
                }
        }

        throw new InvalidOperationException();
    }
    private void SetCell(int x, int y, Tile cell)
    {
        _dungeonMap[x + _mapWidth * y] = cell;
    }
    private Tile GetCellType(int x, int y)
    {
        if (x >= _mapWidth || y >= _mapHeight || x < 0 || y < 0) throw new IndexOutOfRangeException($"({x},{y}) is outside of the map ({_mapWidth},{_mapHeight})");
        try
        {
            return _dungeonMap[x + _mapWidth * y];
        }
        catch (IndexOutOfRangeException)
        {
            throw new IndexOutOfRangeException($"Cords: ({x},{y}) Idx: ({x + _mapWidth * y}) MaxIdx({_mapWidth * _mapHeight}) ");
        }
    }
    private void SetDoor(Vector2 door, Direction d)
    {

        SetCell(door.X, door.Y, Tile.Door);

        int offsetX;
        int offsetY;
        switch (d)
        {
            case Direction.North:
                {
                    offsetY = -1;
                    offsetX = 0;
                    break;
                }
            case Direction.West:
                {
                    offsetX = -1;
                    offsetY = 0;
                    break;
                }
            case Direction.East:
                offsetX = 1;
                offsetY = 0;
                break;
            case Direction.South:
                offsetY = 1;
                offsetX = 0;
                break;
            default:
                offsetX = 0;
                offsetY = 0;
                break;
        }

        SetCell(door.X + offsetX, door.Y + offsetY, Tile.Floor);
    }
    private Room[] GetRoomFromType(int group)
    {
        switch (group)
        {
            case 0:
                return _NormalRooms;
            case 1:
                return _SpecialRooms;
            case 2:
                return _StartingRooms;
            default:
                throw new IndexOutOfRangeException();
        }
    }
    private Direction RandomDirection()
    {
        int dir = _rnd.Next(0, 4);
        switch (dir)
        {
            case 0:
                return Direction.North;
            case 1:
                return Direction.East;
            case 2:
                return Direction.South;
            case 3:
                return Direction.West;
        }

        throw new IndexOutOfRangeException();
    }

    private Direction GetExludedRandomDirection(Direction[] ignore)
    {
        Direction[] allowed = new[]{
            Direction.East,
            Direction.North,
            Direction.South,
            Direction.West
        };
        ImmutableArray<Direction> validDirections = allowed.Where(e => !ignore.Contains(e)).ToImmutableArray();
        int idx = _rnd.Next(0, validDirections.Length);
        return validDirections[idx];
    }

    private BoundingBox? MakeRoom(int x, int y, Direction d, int roomGroup = 0)
    {
        Room[] rooms = GetRoomFromType(roomGroup);

        int roomIdx = _rnd.Next(rooms.Length);
        Room room = rooms[roomIdx];
        int xWidth = room.GetWidth();

        IEnumerable<Vector2> points = room.GetPoints(x, y, d, _rnd);

        if (points.Any(s => s.Y < 0 || s.Y > _mapHeight || s.X < 0 || s.X > _mapWidth || GetCellType(s.X, s.Y) != Tile.Unused)) return null;

        _logger($"Making room:{room.Id}, int y={y}, int x={x}");

        Tile[] tiles = room.GetLayout();

        int row = 0;
        int col = 0;
        foreach (var p in points)
        {
            SetCell(p.X, p.Y, tiles[row + xWidth * col]);
            row++;
            if (row >= xWidth)
            {
                row = 0;
                col++;
            }
        }

        return new BoundingBox
        {
            TopLeft = points.First(),
            BottomRight = points.Last(),
            Width = xWidth,
            Heigth = room.GetHeight()
        };
    }
    private BoundingBox? MakeCorridor(int x, int y, int length, Direction direction)
    {
        int len = _rnd.Next(3, length);

        Corridor corridor = new Corridor(len, direction);

        _logger($"{corridor}");

        IEnumerable<Vector2> points = corridor.GetCorridorPoints(x, y, _fakeRnd);

        if (points.Any(s => s.Y < 0 || s.Y > _mapHeight || s.X < 0 || s.X > _mapWidth || GetCellType(s.X, s.Y) != Tile.Unused)) return null;

        _logger($"Making Corridor:{corridor}, int y={y}, int x={x}");

        Tile[] tiles = corridor.GetLayout();
        int xWidth = corridor.GetWidth();

        int row = 0;
        int col = 0;
        foreach (var p in points)
        {
            SetCell(p.X, p.Y, tiles[row + xWidth * col]);
            row++;
            if (row >= xWidth)
            {
                row = 0;
                col++;
            }
        }

        return new BoundingBox
        {
            TopLeft = points.First(),
            BottomRight = points.Last(),
            Width = xWidth,
            Heigth = corridor.GetHeight()
        };
    }
}