namespace Dungeon.Core;

public class Player
{

    public bool StatsAreDirty { get; set; } = true;

    public int Gold { get; set; } = 0;
    public int Health { get; set; } = 100;
    public string Name { get; set; } = "No Name";
    public int Y { get; set; } = 0;
    public int X { get; set; } = 0;

    public Player(int StartX, int StartY)
    {
        Y = StartY;
        X = StartX;
    }

    public void updateGold(int Value)
    {
        Gold = Value;
        StatsAreDirty = true;
    }
    public void updateHealth(int Value)
    {
        Health = Value;
        StatsAreDirty = true;
    }
    public bool MoveRight(int value = 1)
    {
        if (X >= 50) return false;
        X += value;
        return true;
    }
    public bool MoveLeft(int value = 1)
    {
        if (X <= 1) return false;
        X -= value;
        return true;
    }
    public bool MoveUp(int value = 1)
    {
        if (Y <= 5) return false;
        Y -= value;
        return true;
    }
    public bool MoveDown(int value = 1)
    {
        if (Y >= 23) return false;

        Y += value;
        return true;
    }
}
