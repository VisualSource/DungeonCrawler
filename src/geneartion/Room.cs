using Dungeon.Utils;
namespace Dungeon.Geneartion;

public class Room {
    public required string Id { get; set; }
    public required string[] Layout { get; set; }

    public IEnumerable<Vector2> GetPoints(int originX, int originY, Direction d){
        int width = GetWidth();
        int height = GetHeight();

        int offsetX = 0;
        int offsetY = 0;

        switch (d)
        {
            case Direction.North: {
                offsetX = originX - (width / 2) ;
                offsetY = originY - height - 1;
                break;
            }
            case Direction.West: {
                offsetX = originX - width - 1;
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

    public Vector2 GetTopLeft(int orginX, int originY){
        int width = GetWidth();
        int height = GetHeight();

        int centerX = width / 2;
        int centerY = height / 2;
        return new Vector2(orginX - centerX,originY - centerY);
    }

    public Vector2 GetBottomRight(int originX, int originY){
        int width = GetWidth();
        int height = GetHeight();

        int centerX = width / 2;
        int centerY = height / 2;
        return new Vector2(originX + width - centerX,originY + height - centerY);
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