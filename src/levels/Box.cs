namespace Dungeon.Levels;

public enum EConnection {
    None = 0,
    LeftOnly = 1,
    RightOnly = 2,
    UpOnly = 3,
    DownOnly = 4,
    LeftAndRight = 5,
    UpAndDown = 6,
    All = 7,
    RightDown = 8,
    RightUp = 9,
    LeftDown = 10,
    LeftUp = 11
}
public class Box {
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required string[] Layout { init; get; }
    public EConnection Connections { init; get; } = EConnection.None; 

    public int GetHeight(){
        return Layout.Length;
    }
    public int GetWidth(){
        var a = Layout[0];
        if(a is null) return 0;
        return a.Length;
    }

    public override string ToString(){
        return $"<Box name={Name} connection={Connections}>";
    }
}