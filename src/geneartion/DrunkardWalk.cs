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

        if(points.Any(s=> s.Y < 0 || s.Y > sizeY || s.X < 0 || s.X > sizeX || GetCellType(s.X,s.Y) != Tile.Unused /*|| room.GetCellAt(x - s.X,y - s.Y) != Tile.Unused*/)) return false;

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
    }
}