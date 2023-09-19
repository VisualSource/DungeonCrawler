using Dungeon.Geneartion;

namespace Dungeon.Core;

public class World {
    public string LevelName = "LOADING";
    public World(){}
    public void GenearteLevelName(){
       
    }
    public Tile[] GenearteWorld(){
        DrunkardWalk generator = new DrunkardWalk(e=>Console.WriteLine(e));

        generator.CreateDungeon(150,100,6);

        Console.ReadKey();

        Tile[] map = generator.GetMap();

        generator.Print();

        Console.ReadKey();

        return map;
    }
}