using Dungeon.Geneartion;

namespace Dungeon.Core;

public class World {
    public string LevelName = "LOADING";
    public World(){}
    public void GenearteLevelName(){
       
    }
    public Tile[] GenearteWorld(){
        DrunkardWalk generator = new DrunkardWalk(e=>Console.WriteLine(e));

        generator.CreateDungeon(50,50,2);

        Console.ReadKey();

        Tile[] map = generator.GetMap();
        
        int row = 0;
        foreach(var item in map){

            Console.Write((char)item);
            row++;

            if(row >= 50){
                Console.Write("\n");
                row = 0;
            }
        }

        Console.ReadKey();

        return map;
    }
}