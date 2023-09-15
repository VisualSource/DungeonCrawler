
using System.ComponentModel;
using Dungeon.Utils;
namespace Dungeon.Geneartion;

/// <summary>
///     https://www.roguebasin.com/index.php?title=Dungeon-Building_Algorithm
/// </summary>
public class DrunkardWalk {
    const int ChangeForRoom = 75;
    const int ChangeForSpeialRoom = 10;
    private Random rnd;
    private Tile[] map = {};
    private int sizeX;
    private int sizeY;
    private Room[] NormalRooms;
    private Room[] StartingRooms;
    private Room[] SpecialRooms;

    public DrunkardWalk(Room[] rooms, Room[] startingRooms, Room[] specialRooms, int? seed) {
        rnd = seed is null ? new Random() : new Random((int)seed);
        NormalRooms = rooms;
        StartingRooms = startingRooms;
        SpecialRooms = specialRooms;
    }

    public static int GetFeatureLowerBound(int c, int len){
        return c - len / 2;
    }

    public static int GetFeatureUpperBound(int c, int len){
        return c + (len + 1) / 2;
    }

    public static int InvertGetFeatureLowerBound(int c, int len){
        return (c * 2) + len;
    }

    public static int InvertGetFeatureUpperBound(int c, int len){
        return c + 1 - (len * 2);
    }

    public static IEnumerable<Vector2> GetRoomPoints(int x, int y, int xlen, int ylen, Direction d){
        Func<int,int,int> a = GetFeatureLowerBound;
        Func<int,int,int> b = GetFeatureUpperBound;

        switch (d)
        {
            case Direction.North:
                for (var xt = a(x, xlen); xt < b(x, xlen); xt++) for (var yt = y; yt > y - ylen; yt--) yield return new Vector2(xt,yt);
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

    private bool MakeRoom(int x, int y, Direction direction, int roomType = 0){
        Room[] rooms = GetRoomsFromType(roomType);

        int nextRoom = rnd.Next(rooms.Length);
        Room room = rooms[nextRoom];

        int xLength = room.GetWidth();
        int yLength = room.GetHeight();
        
        var points = GetRoomPoints(x,y,xLength,yLength,direction);

        if(points.Any(s=> s.Y < 0 || s.Y > sizeY || s.X < 0 || s.X > sizeX || GetCellType(s.X,s.Y) != Tile.Unused /*|| room.GetCellAt(x - s.X,y - s.Y) != Tile.Unused*/)) return false;

        Tile[] tiles = room.GetLayout();

        foreach(var p in points){
            SetCell(p.X,p.Y,tiles[TranslateTo1DArray(p.X,p.Y,direction,xLength,yLength,x,y)]);
        }

        return true;
    }

    private int TranslateTo1DArray(int x, int y, Direction d, int xLen, int yLen, int xOffset, int yOffset){
        Func<int,int,int> a = InvertGetFeatureLowerBound;
        Func<int,int,int> b = InvertGetFeatureUpperBound;

        switch (d)
        {
            case Direction.North: {
                return (xOffset - a(x,xLen)) + xLen * (yOffset + y);
            }
            case Direction.East:
                return (x - xOffset) + xLen * (y - yOffset);
            case Direction.South:
                return (a(x,xLen) - xOffset) + xLen * ( b(y,yLen) - yOffset); 
            case Direction.West:
                return (x + xOffset) + xLen * (a(y,yLen) - yOffset);

            default: 
                return 0;
        }
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

    public void Genearte(int x, int y){
        sizeX = x;
        sizeY = y;

        map = new Tile[sizeX * sizeY];

        Initialize();

        MakeRoom(sizeX / 2, sizeY / 2,RandomDirection(),1);

        int currentFeatures = 1;

    }
}