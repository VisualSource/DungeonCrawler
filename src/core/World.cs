
using System.Collections.Immutable;
using System.Text.Json;
using Dungeon.Geneartion;

namespace Dungeon.Core;

public class World {
    public string LevelName = "LOADING";

    private Room[] NormalRooms;
    private Room[] StaringRooms;
    private Room[] SpecialRooms;
    public World(){
        NormalRooms = LoadRooms("NormalRooms.json");
        StaringRooms = LoadRooms("StartingRooms.json");
        SpecialRooms = new Room[0];
    }

    private Room[] LoadRooms(string path){
        string rawJson = File.ReadAllText(path);
        var rooms = JsonSerializer.Deserialize<Room[]>(rawJson);

        if(rooms is null) throw new Exception($"Failed to load file from {path}");

        return rooms;
    }

    public void GenearteLevelName(){
       
    }
    public Tile[] GenearteWorld(){
        DrunkardWalk generator = new DrunkardWalk(e=>Console.WriteLine(e));

        generator.CreateDungeon(50,50,1);

        Console.ReadKey();

        return generator.GetMap();
    }
}