using System.Diagnostics.CodeAnalysis;
using System.Text;
using Dungeon.Utils;

namespace Dungeon.Geneartion;

public class Corridor: Room {
    public int Length = 0;
    public Direction Dir = Direction.North;

    [SetsRequiredMembers]
    public Corridor(int length, Direction direction) {
        Length = length;
        Dir = direction;
        Id = "Corridor";
        Layout = Init().ToArray();
    }
    private IEnumerable<string> Init(){
        switch (Dir)
        {   
            case Direction.South:
            case Direction.North: {
                for (int y = 0; y < Length; y++)
                {
                   if(y == 0 || y == (Length - 1)) {
                        yield return "###";
                        continue;
                   }
                   yield return "#.#";
                }
                break;
            }
            case Direction.West:
            case Direction.East: {
                StringBuilder stringBuilder = new StringBuilder(Length);

                for (int y = 0; y < 3; y++)
                {
                    stringBuilder.Clear();

                    for (int x = 0; x < Length; x++)
                    {
                        if(x == 0 || x == (Length - 1) || y % 2 == 0){
                            stringBuilder.Append('#');
                            continue;
                        }

                        stringBuilder.Append('.');
                    }

                   yield return stringBuilder.ToString();  
                }
                break;
            }
            default:
                throw new IndexOutOfRangeException();
        }

        yield break;
    }

    public IEnumerable<Vector2> GetCorridorPoints(int originX, int originY, IRandom rnd){
        return GetPoints(originX, originY,Dir,rnd);
    }
    public override string ToString(){
        return $"<Corridor Length={Length} Direction={Dir}/>";
    }
}