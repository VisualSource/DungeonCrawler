using Dungeon.Utils;
namespace Dungeon.Geneartion;

public class Room {
    public required string Id { get; init; }
    public required string[] Layout { get; init; }
    public IEnumerable<Vector2> GetStartingOffset(Direction d){
        int width = GetWidth();
        int height = GetHeight();

        switch (d)
        {
            case Direction.South:
            case Direction.North: {

                Vector2? currentRange = null;
                int wall = d == Direction.North ? height - 1 : 0;
                int floor = d == Direction.North ? height - 2 : 1;
            
                for (int x = 0; x < width; x++)
                {
                    if(GetCellAt(x,wall) == Tile.Wall && GetCellAt(x,floor) == Tile.Floor){
                        if(currentRange is null){
                            currentRange = new Vector2(x,-1);
                        }
                        continue;
                    }

                    if(currentRange is null) continue;

                    currentRange.Y = x - 1;
                    yield return currentRange;
                    currentRange = null;
                }

                if(currentRange is not null){
                    currentRange.Y = width - 1;
                    yield return currentRange;
                }

                break;
            }
            case Direction.West:
            case Direction.East: {
                int wall = d == Direction.East ? 0 : width - 1;
                int floor = d == Direction.East ? 1 : width - 2;

                Vector2? currentRange = null;
                for (int y = 0; y < height; y++)
                {
                    if(GetCellAt(wall,y) == Tile.Wall && GetCellAt(floor,y) == Tile.Floor){
                        if(currentRange is null){
                            currentRange = new Vector2(y,-1);
                        }
                        continue;
                    } 

                    if(currentRange is null) continue;
                    currentRange.Y = y - 1;
                    yield return currentRange;
                    currentRange = null;
                }

                if(currentRange is not null && currentRange.Y == -1){
                    currentRange.Y = height - 2;
                    yield return currentRange;
                }
            }
            break;
            default:
                throw new IndexOutOfRangeException();
        }

        yield break;
    }

    public IEnumerable<Vector2> GetPoints(int originX, int originY, Direction d, IRandom rnd){
        int width = GetWidth();
        int height = GetHeight();
        IEnumerable<Vector2> ranges = GetStartingOffset(d);

        int rangeCount = ranges.Count();
        if(rangeCount <= 0) throw new ArgumentOutOfRangeException("Insufficient exit door ranges.");
    
        int idx = rnd.Next(rangeCount);

        Vector2 range = ranges.ElementAt(idx);

        int offset = rnd.Next(range.X,range.Y);
        return InternalGetPoints(originX,originY,d,offset,width,height);
    }

    private IEnumerable<Vector2> InternalGetPoints(int originX, int originY, Direction d, int offset, int width, int height){
        int offsetX;
        int offsetY;

        switch (d)
        {
            case Direction.North: {
                offsetX = originX - offset;
                offsetY = originY - height;
                break;
            }
            case Direction.West: {
                offsetX = originX - width;
                offsetY = originY - offset;
                break;
            }
            case Direction.East: {
                offsetX = originX + 1;
                offsetY = originY - offset;
                break;
            }
            case Direction.South: {
                offsetX = originX - offset;
                offsetY = originY + 1;
                break;
            }
            default:
                offsetX = originX;
                offsetY = originY;
                break;
        }

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                yield return new Vector2(offsetX + x, offsetY + y);
            }
        }

        yield break;
    }
    public Tile GetCellAt(int x, int y){
        return (Tile)Layout[y][x];
    }

    public int GetWidth(){
        var item = Layout[0];

        if(item is null) throw new Exception("No item exists at this index");

        return item.Length;
    }
    public int GetHeight(){
        return Layout.Length;
    }

    public Tile[] GetLayout(){
        int width = GetWidth();
        int height = GetHeight();
        Tile[] room = new Tile[width * height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                room[x + width * y] = (Tile)Layout[y][x];
            }
        }

        return room;
    }

    public override string ToString(){
        return $"<Room Id={Id}/>";
    }
}