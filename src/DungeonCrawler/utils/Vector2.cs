using System.Reflection;

namespace Dungeon.Utils;

[DefaultMember("Item")]
public class Vector2: IFormattable, IEquatable<Vector2> {
    public int X;
    public int Y;
    public Vector2(int X, int Y){
        this.X = X;
        this.Y = Y;
    }

    public bool Equals(Vector2? other)
    {
        if(other is null) return false;
        return other.Y == Y && other.X == X;
    }
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return $"<Vector X={X} Y={Y}/>";
    }
}