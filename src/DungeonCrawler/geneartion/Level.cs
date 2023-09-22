using System.Diagnostics.CodeAnalysis;
using Dungeon.Utils;

namespace Dungeon.Geneartion;

public class Level
{
    public required string Id { get; init; }

    public required string Name { get; init; }

    public required Tile[] Map { get; set; }

    public required Tuple<string, Vector2>[] Ladders { get; set; }

    public required Vector2 StartPoint { get; init; }

    [SetsRequiredMembers]
    public Level(string id, string name, Tile[] map, Tuple<string, Vector2>[] ladders, Vector2 startPoint)
    {
        Id = id;
        Name = name;
        Map = map;
        Ladders = ladders;
        StartPoint = startPoint;
    }
}