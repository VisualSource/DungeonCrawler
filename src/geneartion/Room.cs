using Dungeon.Utils;
namespace Dungeon.Geneartion;

public class Room {
    public required string Id { get; set; }
    public required string[] Layout { get; set; }


    public IEnumerable<Vector2> GetPoints(int originX, int originY){
        int width = GetWidth();
        int height = GetHeight();

        int centerX = width / 2;
        int centerY = height / 2;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                yield return new Vector2(originX + x - centerX , originY + y - centerY);
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