using System.Reflection.Emit;
using Dungeon.Utils;
namespace Dungeon.Geneartion;

public class Room {
    public required string Id { get; set; }
    public required string[] Layout { get; set; }

    public Vector2[] GetStartingOffset(Direction d){
        Vector2[] ranges = new Vector2[]{}; 
        int width = GetWidth();
        int height = GetHeight();

        switch (d)
        {
            case Direction.North:
                break;
            case Direction.South:
                break;
            case Direction.East: {
                
                Vector2? currentRange = null;
                for (int y = 0; y < height; y++)
                {
                    if(GetCellAt(width - 1,y) == Tile.Wall && GetCellAt(width - 2,y) == Tile.Floor){
                        if(currentRange is null){
                            currentRange = new Vector2(y,-1);
                        }
                        continue;
                    } 

                    if(currentRange is null) continue;
                    currentRange.Y = y - 1;
                    ranges.Append(currentRange);
                    currentRange = null;
                }

                if(currentRange is not null && currentRange.Y == -1){
                    currentRange.Y = height - 1;
                }
            }
                break;
            case Direction.West:
                break;
            default:
                throw new IndexOutOfRangeException();
        }




        return ranges;
    }

    
    public IEnumerable<Vector2> GetPoints(int originX, int originY, Direction d){
        int width = GetWidth();
        int height = GetHeight();

        int offsetX;
        int offsetY;

        switch (d)
        {
            case Direction.North: {
                offsetX = originX - (width / 2) ;
                offsetY = originY - height;
                break;
            }
            case Direction.West: {
                offsetX = originX - width;
                offsetY = originY - (height / 2);
                break;
            }
            case Direction.East: {
                offsetX = originX + 1;
                offsetY = originY - (height / 2);
                break;
            }
            case Direction.South: {
                offsetX = originX - (width / 2);
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