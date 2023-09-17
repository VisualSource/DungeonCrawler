using System.Collections.Immutable;
using System.Reflection.Metadata;
using System.Text.Json;
using Dungeon.Utils;
namespace Dungeon.Geneartion;

/*public class DrunkardWalk {
    const int ChangeForRoom = 75;
    const int ChangeForSpeialRoom = 10;
    private Random rnd;

    private readonly Action<string> logger;
    private Tile[] map = {};

    private int features = 10;
    private int sizeX;
    private int sizeY;

    private int maxX = 50;
    private int maxY = 50;
    private Room[] NormalRooms;
    private Room[] StartingRooms;
    private Room[] SpecialRooms;
    public int Corridors
    {
            get;
            private set;
    }
    public DrunkardWalk(Room[] rooms, Room[] startingRooms, Room[] specialRooms, Action<string> logger, int? seed) {
        rnd = seed is null ? new Random() : new Random((int)seed);
        NormalRooms = rooms;
        StartingRooms = startingRooms;
        SpecialRooms = specialRooms;
        this.logger = logger;
    }

    public static int GetFeatureLowerBound(int c, int len){
        return c - len / 2;
    }

    public static int GetFeatureUpperBound(int c, int len){
        return c + (len + 1) / 2;
    }

    public static IEnumerable<Vector2> GetRoomPoints(int x, int y, int xlen, int ylen, Direction d){
        Func<int,int,int> a = GetFeatureLowerBound;
        Func<int,int,int> b = GetFeatureUpperBound;

        switch (d)
        {
            case Direction.North:
                for (var xt = a(x, xlen); xt < b(x, xlen); xt++){
                    for (var yt = y; yt > y - ylen; yt--){
                        yield return new Vector2(xt,yt);
                    }
                }
                break;
            case Direction.East:
                for (var xt = x; xt < x + xlen; xt++) for (var yt = a(y, ylen); yt < b(y, ylen); yt++) yield return new Vector2(xt, yt);
                break;
            case Direction.South:
                for (var xt = a(x, xlen); xt < b(x, xlen); xt++) for (var yt = y; yt < y + ylen; yt++) yield return new Vector2(xt,yt);
                break;
            case Direction.West:
                for (var xt = x; xt > x - xlen; xt--) for (var yt = a(y, ylen); yt < b(y, ylen); yt++) yield return new Vector2(xt,yt);
                break;
            case Direction.None:
                for (int i = 0; i < ylen; i++)
                {
                    for (int j = 0; j < xlen; j++)
                    {
                        yield return new Vector2(j + x, i + y);
                    }
                }

                break;
            default:
                yield break;
        }
    }

    public Tile[] GetMap(){
        return map;
    }

    Direction RandomDirection(){
        int dir = rnd.Next(4);
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
            default:
                throw new InvalidOperationException();
        }
    }

    Tile GetCellType(int x, int y){
        try
        {
            return map[x + sizeX * y];
        }
        catch (IndexOutOfRangeException)
        {
            Console.WriteLine($"{x},{y} is out of range.");
            throw;
        }
    }

    private void SetCell(int x, int y, Tile cell){
        map[x + sizeX * y] = cell;
    }
    private Room[] GetRoomsFromType(int type){
        switch (type)
        {
            case 1: 
                return StartingRooms;
            case 2:
                return SpecialRooms;            
            default:
                return NormalRooms;
        }
    }

    private bool MakeCorridor(int x, int y, int length, Direction direction){
          // define the dimensions of the corridor (er.. only the width and height..)
            int len =  rnd.Next(2, length);
            const Tile Floor = Tile.Corridor;
    
            int xtemp;
            int ytemp = 0;
    
            switch (direction)
            {
                case Direction.North:
                    // north
                    // check if there's enough space for the corridor
                    // start with checking it's not out of the boundaries
                    if (x < 0 || x > sizeX) return false;
                    xtemp = x;
    
                    // same thing here, to make sure it's not out of the boundaries
                    for (ytemp = y; ytemp > (y - len); ytemp--)
                    {
                        if (ytemp < 0 || ytemp > sizeY) return false; // oh boho, it was!
                        if (GetCellType(xtemp, ytemp) != Tile.Unused) return false;
                    }
    
                    // if we're still here, let's start building
                    Corridors++;
                    for (ytemp = y; ytemp > (y - len); ytemp--)
                    {
                        SetCell(xtemp, ytemp, Floor);
                    }
    
                    break;
    
                case Direction.East:
                    // east
                    if (y < 0 || y > sizeY) return false;
                    ytemp = y;
    
                    for (xtemp = x; xtemp < (x + len); xtemp++)
                    {
                        if (xtemp < 0 || xtemp > sizeX) return false;
                        if (GetCellType(xtemp, ytemp) != Tile.Unused) return false;
                    }
    
                    Corridors++;
                    for (xtemp = x; xtemp < (x + len); xtemp++)
                    {
                       SetCell(xtemp, ytemp, Floor);
                    }
    
                    break;
    
                case Direction.South:
                    // south
                    if (x < 0 || x > sizeX) return false;
                    xtemp = x;
                    
                    for (ytemp = y; ytemp < (y + len); ytemp++)
                    {
                        if (ytemp < 0 || ytemp > sizeY) return false;
                        if (GetCellType(xtemp, ytemp) != Tile.Unused) return false;
                    }
                    
                    Corridors++;
                    for (ytemp = y; ytemp < (y + len); ytemp++)
                    {
                       SetCell(xtemp, ytemp, Floor);
                    }
                    
                    break;
                case Direction.West:
                    // west
                    if (ytemp < 0 || ytemp > sizeY) return false;
                    ytemp = y;
                    
                    for (xtemp = x; xtemp > (x - len); xtemp--)
                    {
                        if (xtemp < 0 || xtemp > sizeX) return false;
                        if (GetCellType(xtemp, ytemp) != Tile.Unused) return false;
                    }
                    
                    Corridors++;
                    for (xtemp = x; xtemp > (x - len); xtemp--)
                    {
                        SetCell(xtemp, ytemp, Floor);
                    }
                    
                    break;
            }

            // woot, we're still here! let's tell the other guys we're done!!
            return true;
    }
    private bool MakeRoom(int x, int y, Direction direction, int roomType = 0){
        Room[] rooms = GetRoomsFromType(roomType);

        int nextRoom = rnd.Next(rooms.Length);
        Room room = rooms[nextRoom];

        int roomWidth = room.GetWidth();
        int roomHeight = room.GetHeight();

        var points = room.GetPoints(x,y);

        if(points.Any(s=> s.Y < 0 || s.Y > sizeY || s.X < 0 || s.X > sizeX || GetCellType(s.X,s.Y) != Tile.Unused) return false;

        Tile[] tiles = room.GetLayout();

        int col = 0;
        int row = 0;
        foreach(var p in points){
            
            SetCell(p.X,p.Y,tiles[row + roomWidth * col]);

            row++;
            if(row >= roomWidth){
                col++;
                row = 0;
            }
        }

        return true;
    }

    private void Initialize(){
        for (int i = 0; i < sizeY; i++)
        {   
            for (int x = 0; x < sizeX; x++)
            {
                SetCell(x,i,Tile.Unused);
            }
        }
    }

    public bool InBounds(Vector2 v){
        return v.X > 0 && v.X < maxX && v.Y > 0 && v.Y < maxY;
    }

    public IEnumerable<Tuple<Vector2,Direction>> GetSurroundingPoints(Vector2 v){
        var points = new[]{
            Tuple.Create(new Vector2(v.X,v.Y + 1),Direction.North),
            Tuple.Create(new Vector2(v.X - 1,v.Y),Direction.East),
            Tuple.Create(new Vector2(v.X,v.Y -1),Direction.South),
            Tuple.Create(new Vector2(v.X + 1, v.Y),Direction.West)
        };

        return points.Where(p=> InBounds(p.Item1));
    }

    public IEnumerable<Tuple<Vector2,Direction,Tile>> GetSurroundings(Vector2 v){
        return GetSurroundingPoints(v).Select(r => Tuple.Create(r.Item1,r.Item2,GetCellType(r.Item1.X,r.Item1.Y)));
    }

    public void Genearte(int x, int y){
        sizeX = x;
        sizeY = y;

        map = new Tile[sizeX * sizeY];

        Initialize();

        MakeRoom(sizeX / 2, sizeY / 2,RandomDirection(),1);

        int currentFeatures = 1;

        for(int countingTries = 0; countingTries < 1000; countingTries++){
            if(currentFeatures == features){
                break;
            }

            int newX = 0;
            int newY = 0;
            int modX = 0;
            int modY = 0;
            Direction? validTile = null;

            for(int testing = 0; testing < 1000; testing++){
                newX = rnd.Next(1,sizeX - 1);
                newY = rnd.Next(1,sizeY - 1);

                if(GetCellType(newX,newY) == Tile.Corridor || GetCellType(newX,newY) == Tile.Wall){
                    var surrounding = GetSurroundings(new Vector2(newX,newY));

                    var canReach = surrounding.FirstOrDefault(s => s.Item3 == Tile.Floor || s.Item3 == Tile.Corridor);

                    if(canReach == null) {
                        continue;
                    }

                    validTile = canReach.Item2;

                    switch (canReach.Item2)
                    {
                        case Direction.North:
                            modX = 0;
                            modY = -1;
                            break;
                        case Direction.East:
                            modX = 1;
                            modY = 0;
                            break;
                        case Direction.South:
                            modX = 0;
                            modY = 1;
                            break;
                        case Direction.West:
                            modX = -1;
                            modY = 0;
                            break;
                        default:
                            throw new InvalidOperationException();
                    }

                    if (GetCellType(newX, newY + 1) == Tile.Door) // north
                    {
                        validTile = null;
                    } else if (GetCellType(newX - 1, newY) == Tile.Door) // east
                        validTile = null;
                    else if (GetCellType(newX, newY - 1) == Tile.Door) // south
                        validTile = null;
                    else if (GetCellType(newX + 1, newY) == Tile.Door) // west
                        validTile = null;


                    if (validTile.HasValue) break;
                }
            }

            if(validTile.HasValue){
                int feature = rnd.Next(0, 100);
                if(feature <= ChangeForRoom){
                    if(MakeRoom(newX + modX, newY + modY, validTile.Value)){
                        currentFeatures++;
                        SetCell(newX,newY,Tile.Door);
                        SetCell(newX+modX,newY+modY,Tile.Floor);
                    }
                } else if(feature >= ChangeForRoom){
                    if(MakeCorridor(newX + modX,newY + modY,6,validTile.Value)){
                        currentFeatures++;
                        SetCell(newX,newY,Tile.Door);
                    }
                }
            }
        }


        Console.WriteLine($"Finished: generated {currentFeatures} features and {Corridors} Corridors");
        Console.ReadKey();

        int row = 0;
        foreach (var item in map)
        {
            Console.Write((char)item);

            row++;

            if(row >= sizeX){
                row = 0;
                Console.Write("\n");
            }
        }

        Console.ReadKey();
    }
}*/

/// <summary>
///     https://www.roguebasin.com/index.php?title=Dungeon-Building_Algorithm
///     https://www.roguebasin.com/index.php/CSharp_Example_of_a_Dungeon-Building_Algorithm
/// </summary>
public class DrunkardWalk {

    struct BoundingBox {
        public Vector2 TopLeft { get; init; }
        public Vector2 BottomRight { get; init; }
        public int Width { get; init; }
        public int Heigth { get; init; }

        public override string ToString(){
            return $"<BoundingBox Width={Width} Height={Heigth} TopLeft={TopLeft} BottomRight={BottomRight}/>";
        } 
    }

    private const int SpecialRoomChance = 10;
    private const int ChanceRoom = 75;
    public int MapMaxWidth = 80;
    public int MapMaxHeight = 25;
    private int _mapWidth;
    private int _mapHeight;
    private int _features;
    private Tile[] _dungeonMap = {};
    private readonly Random _rnd = new Random();
    private readonly Action<string> _logger;
    private readonly Room[] _NormalRooms;
    private readonly Room[] _StartingRooms;
    private readonly Room[] _SpecialRooms;
    public DrunkardWalk(Action<string> logger){
        _logger = logger;

        _NormalRooms = LoadRoomFile("NormalRooms.json");
        _StartingRooms = LoadRoomFile("StartingRooms.json");
        _SpecialRooms = LoadRoomFile("SpecialRooms.json");
    } 
    private Room[] LoadRoomFile(string path){
        string raw = File.ReadAllText(path);
        Room[]? rooms = JsonSerializer.Deserialize<Room[]>(raw);

        if(rooms is null) throw new FileLoadException($"Failed to load file {path}");

        return rooms;
    }
    public Tile[] GetMap(){
        return _dungeonMap;
    }
    public void CreateDungeon(int inX, int inY, int inFeatures){
        if(inFeatures < 1 || inX < 10 || inY < 10) throw new ArgumentOutOfRangeException();

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
                SetCell(x,y,Tile.Unused);
            }
        }

        Direction currentDirection = RandomDirection();
        Vector2? currentExit = null;
        BoundingBox? currentFeature = null;
        int currentFeatures = 0;

        currentFeature = MakeRoom(_mapWidth / 2, _mapHeight / 2, RandomDirection(), 2);

        _logger($"Current Direction: {currentDirection} {currentFeature}");
        if(currentFeature is null) throw new Exception("Failed to init map");
        currentFeatures++;

        currentExit = GetRoomExit(currentFeature.Value,currentDirection);
   
            
        for(int countingTries = 0; countingTries < 1000; countingTries++){
            if(currentFeatures == _features){
                break;
            }   

            if(currentExit is null) continue;

            int feature = _rnd.Next(0,100);
                
            if(feature <= ChanceRoom){
                int special = _rnd.Next(0,100);

                if(special >= SpecialRoomChance){
                    continue;
                }

                BoundingBox? nextFeature = MakeRoom(currentExit.X, currentExit.Y,currentDirection,1);

                if(!nextFeature.HasValue) {
                    throw new Exception("Unable to place feature");
                }

                currentFeature = nextFeature;

                Direction nextDirection = GetExludedRandomDirection(new Direction[]{ currentDirection });

                currentExit = GetRoomExit(currentFeature.Value, nextDirection);

                currentDirection = nextDirection;
        
                continue;
            } else if(feature <= ChanceRoom){
                BoundingBox? nextFeature = MakeCorridor(currentExit.X,currentExit.Y,8,currentDirection);

                if(!nextFeature.HasValue){
                    throw new Exception("Unable to place corridor");
                }

                currentFeature = nextFeature;
                Direction nextDirection = GetExludedRandomDirection(new Direction[]{ currentDirection });
                currentExit = GetRoomExit(currentFeature.Value,nextDirection);
            }
        }
    }

    private Vector2 GetRoomExit(BoundingBox bounding, Direction d){
        Vector2 topLeft = bounding.TopLeft;
        Vector2 bottomRight = bounding.BottomRight;
        int width = bounding.Width;
        int height = bounding.Heigth;

        switch (d)
        {
            case Direction.North: {
                for (int x = 0; x < width; x++)
                {
                    int idx = _rnd.Next(topLeft.X + 1,bottomRight.X - 1);
                    _logger($"Idx {idx},{topLeft.Y} {GetCellType(idx,topLeft.Y)}");
                    if(GetCellType(idx,topLeft.Y) != Tile.Wall) continue;
                    _logger("OK");
                    return new Vector2(idx, topLeft.Y);
                }

                throw new IndexOutOfRangeException("Unable to make a door in the given direction");
            }
            case Direction.South: {
                for (int x = 0; x < width; x++)
                {
                    int idx = _rnd.Next(topLeft.X + 1, bottomRight.X - 1);
                    _logger($"Idx {idx},{topLeft.Y} {GetCellType(idx,bottomRight.Y)}");
                    if(GetCellType(idx,bottomRight.Y) != Tile.Wall) continue;
                    _logger("OK");
                    return new Vector2(idx, bottomRight.Y);
                }
                throw new IndexOutOfRangeException("Unable to make a door in the given direction");
            }
            case Direction.West: {
                for (int y = 0; y < height; y++)
                {
                    int idx = _rnd.Next(topLeft.Y + 1, bottomRight.Y - 1);
                    _logger($"Idx {topLeft.X},{idx} {GetCellType(topLeft.X,idx)}");
                    if(GetCellType(topLeft.X,idx) != Tile.Wall) continue;
                    _logger("OK");
                    return new Vector2(topLeft.X,idx);
                }

                throw new IndexOutOfRangeException("Unable to make a door in the given direction");
            }
            case Direction.East: {
                for (int y = 0; y < height; y++)
                {
                    int idx = _rnd.Next(topLeft.Y + 1, bottomRight.Y - 1);
                    _logger($"Idx {topLeft.X},{idx} {GetCellType(bottomRight.X,idx)}");
                    if(GetCellType(bottomRight.X,idx) != Tile.Wall) continue;
                    _logger("OK");
                    return new Vector2(bottomRight.X,idx);
                }
                throw new IndexOutOfRangeException("Unable to make a door in the given direction");
            }
        }

        throw new InvalidOperationException();
    }


    private void SetCell(int x, int y, Tile cell){
        _dungeonMap[x + _mapWidth * y] = cell;
    }

    private Tile GetCellType(int x, int y){
        return _dungeonMap[x + _mapWidth * y];
    }

    private Room[] GetRoomFromType(int group){
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

    private Direction RandomDirection(){
        int dir = _rnd.Next(0,4);
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

    private Direction GetExludedRandomDirection(Direction[] ignore){
        Direction[] allowed = new[]{
            Direction.East,
            Direction.North,
            Direction.South,
            Direction.West
        };
        ImmutableArray<Direction> validDirections = allowed.Where(e=> !ignore.Contains(e)).ToImmutableArray();
        int idx = _rnd.Next(0,validDirections.Length);
        return validDirections[idx];
    }

    private BoundingBox? MakeRoom(int x, int y, Direction d, int roomGroup = 0){
        Room[] rooms = GetRoomFromType(roomGroup);
        
        int roomIdx = _rnd.Next(rooms.Length);
        Room room = rooms[roomIdx];
        int xWidth = room.GetWidth();

        IEnumerable<Vector2> points = room.GetPoints(x,y,d);
    
        if(points.Any(s=> s.Y < 0 || s.Y > _mapHeight || s.X < 0 || s.X > _mapWidth || GetCellType(s.X,s.Y) != Tile.Unused)) return null;

        _logger($"Making room:{room.Id}, int y={y}, int x={x}");

        Tile[] tiles = room.GetLayout();

        int row = 0;
        int col = 0;
        foreach(var p in points){
            SetCell(p.X,p.Y,tiles[row + xWidth * col]);
            row++;
            if(row >= xWidth){
                row = 0;
                col++;
            }
        }

        return new BoundingBox {  
            TopLeft = room.GetTopLeft(x,y),
            BottomRight= room.GetBottomRight(x,y),
            Width = room.GetWidth(),
            Heigth = room.GetHeight()
        };
    }
    private BoundingBox? MakeCorridor(int x, int y, int length, Direction direction){
        int len = _rnd.Next(2,length);
        
        int xtemp;
        int ytemp = 0;

        switch (direction)
        {
            case Direction.North:
                    // north
                    // check if there's enough space for the corridor
                    // start with checking it's not out of the boundaries
                    if (x < 0 || x > _mapWidth) return null;
                    xtemp = x;
    
                    // same thing here, to make sure it's not out of the boundaries
                    for (ytemp = y; ytemp > (y - len); ytemp--)
                    {
                        if (ytemp < 0 || ytemp > _mapHeight) return null; // oh boho, it was!
                        if (GetCellType(xtemp, ytemp) != Tile.Unused) return null;
                    }
    
                    // if we're still here, let's start building
                    for (ytemp = y; ytemp > (y - len); ytemp--)
                    {
                        SetCell(xtemp, ytemp, Tile.Floor);
                    }
    
                    return new BoundingBox{
                        Width = 1,
                        Heigth = len,
                        TopLeft = new Vector2(x, y - len),
                        BottomRight = new Vector2(x,y)
                    };
    
            case Direction.East:
                    // east
                    if (y < 0 || y > _mapHeight) return null;
                    ytemp = y;
    
                    for (xtemp = x; xtemp < (x + len); xtemp++)
                    {
                        if (xtemp < 0 || xtemp > _mapWidth) return null;
                        if (GetCellType(xtemp, ytemp) != Tile.Unused) return null;
                    }
    
                    for (xtemp = x; xtemp < (x + len); xtemp++)
                    {
                        SetCell(xtemp, ytemp, Tile.Floor);
                    }
    
                    return new BoundingBox {
                        Heigth = 1,
                        Width = len,
                        TopLeft = new Vector2(x,y),
                        BottomRight = new Vector2(x+len,y)
                    };
    
            case Direction.South:
                    // south
                    if (x < 0 || x > _mapWidth) return null;
                    xtemp = x;
                    
                    for (ytemp = y; ytemp < (y + len); ytemp++)
                    {
                        if (ytemp < 0 || ytemp > _mapHeight) return null;
                        if (GetCellType(xtemp, ytemp) != Tile.Unused) return null;
                    }
                    
                    for (ytemp = y; ytemp < (y + len); ytemp++)
                    {
                        SetCell(xtemp, ytemp, Tile.Floor);
                    }
                    
                    return new BoundingBox{
                        Heigth = len,
                        Width = 1,
                        TopLeft = new Vector2(x,y),
                        BottomRight = new Vector2(x,y+len)
                    };
            case Direction.West:
                    // west
                    if (ytemp < 0 || ytemp > _mapHeight) return null;
                    ytemp = y;
                    
                    for (xtemp = x; xtemp > (x - len); xtemp--)
                    {
                        if (xtemp < 0 || xtemp > _mapWidth) return null;
                        if (GetCellType(xtemp, ytemp) != Tile.Unused) return null;
                    }
                    
                    for (xtemp = x; xtemp > (x - len); xtemp--)
                    {
                        SetCell(xtemp, ytemp, Tile.Floor);
                    }
                    
                    return new BoundingBox {
                        Heigth = 1,
                        Width = len,
                        TopLeft = new Vector2(x - len,y),
                        BottomRight = new Vector2(x,y)
                    };
        }

        throw new InvalidOperationException();
    }
}