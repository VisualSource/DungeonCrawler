using Dungeon.Levels;
using System.Collections.Immutable;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Timers;
namespace Dungeon.Core;

public class World {

    static EConnection[] AboveEmpty = new EConnection[]{
        EConnection.UpOnly,
        EConnection.UpAndDown,
        EConnection.RightUp,
        EConnection.LeftUp,
        EConnection.All
    };
    static EConnection[] AboveFilled = new EConnection[]{
        EConnection.None,
        EConnection.LeftOnly,
        EConnection.RightOnly,
        EConnection.LeftAndRight,
        EConnection.RightDown,
        EConnection.LeftDown
    };

    // Left to Right
    private EConnection[][] AllowedLookUp = {
        new EConnection[]{
            EConnection.RightOnly,
            EConnection.UpOnly,
            EConnection.DownOnly,
            EConnection.UpAndDown,
            EConnection.RightDown,
            EConnection.RightUp
        },//None
        new EConnection[]{
            EConnection.None,
            EConnection.RightOnly,
            EConnection.DownOnly,
            EConnection.UpOnly,
            EConnection.UpAndDown,
            EConnection.RightDown,
            EConnection.RightDown
        },//LeftOnly
        new EConnection[]{ 
            EConnection.LeftOnly,
            EConnection.LeftAndRight,
            EConnection.All,
            EConnection.LeftDown,
            EConnection.LeftUp
        },// RightOnly
        new EConnection[]{  
           EConnection.None,
           EConnection.RightOnly,
           EConnection.UpAndDown,
           EConnection.RightDown,
           EConnection.RightUp,
        },//UpOnly
        new EConnection[]{
            EConnection.None,
            EConnection.RightOnly,
            EConnection.UpAndDown,
            EConnection.RightDown,
            EConnection.RightUp
        },//DownOnly
        new EConnection[]{
          EConnection.LeftOnly,
          EConnection.LeftAndRight,
          EConnection.All,
          EConnection.LeftDown,
          EConnection.LeftUp
        },//LeftAndRight
        new EConnection[]{
            EConnection.None,
            EConnection.RightOnly,
            EConnection.UpOnly,
            EConnection.DownOnly,
            EConnection.UpAndDown,
            EConnection.RightDown,
            EConnection.RightUp
        },//UpAndDown
        new EConnection[]{
            EConnection.LeftOnly,
            EConnection.LeftAndRight,
            EConnection.All,
            EConnection.LeftDown,
            EConnection.LeftUp
        },//All
        new EConnection[]{
            EConnection.LeftOnly,
            EConnection.LeftAndRight,
            EConnection.All,
            EConnection.LeftDown,
            EConnection.LeftUp
        },// RightDown
        new EConnection[]{
            EConnection.LeftOnly,
            EConnection.LeftDown,
            EConnection.LeftUp,
            EConnection.All,
            EConnection.LeftAndRight
        },//Rightup
        new EConnection[]{
            EConnection.None,
            EConnection.RightOnly,
            EConnection.UpOnly,
            EConnection.DownOnly,
            EConnection.UpAndDown,
            EConnection.RightDown,
            EConnection.RightUp
        },//LeftDown
        new EConnection[]{
            EConnection.None,
            EConnection.RightOnly,
            EConnection.UpOnly,
            EConnection.DownOnly,
            EConnection.UpAndDown,
            EConnection.RightDown,
            EConnection.RightUp
        }
    } ;
    private EConnection[][] AboveLookUp = {
        AboveFilled,// None
        AboveFilled,//LeftOnly
        AboveFilled,//RightOnly
        AboveFilled,//UpOnly
        AboveEmpty,//DownOnly
        AboveFilled,//LeftAndRight
        AboveEmpty,//UpAndDown
        AboveEmpty,//All
        AboveFilled,//RightDown
        AboveFilled,//RightUp
        AboveEmpty,//LeftDown
        AboveFilled//LeftUp
    };

    public string LevelName = "LOADING";
    public List<string> Level = new List<string> {};

    private Box[] Layouts;

    public World(long seed){
        string fileName = "LevelData.json";
        string jsonString = File.ReadAllText(fileName);
        var layouts = JsonSerializer.Deserialize<Box[]>(jsonString);
        
        if(layouts is null) {
            Layouts = new Box[]{};
        } else {
            Layouts = layouts;
        }
    }

    public void GenearteLevelName(){
       
    }
    public void GenearteWorld(){
        var rand = new Random();
      
        Console.WriteLine("GENERATING");

        int[] prevAbove = new int[]{ 0, 0, 0 ,0 ,0 };
        for (int y = 0; y < 6; y++)
        {
            int prevItem = 0;
            for (int x = 0; x < 5; x++)
            {
                var allowed = Layouts.Where(x => AllowedLookUp[prevItem].Contains(x.Connections)).Where(e => AboveLookUp[prevAbove[x]].Contains(e.Connections)).ToImmutableArray();
    
                var item = rand.Next(allowed.Count());
                var layout = allowed[item];

                if(layout is null) continue;

                prevItem = item;
                for (int yj = 0; yj < 3; yj++)
                {
                    int index = y * 3 + yj;

                    if(index >= Level.Count){
                        Level.Add(layout.Layout[yj]);
                    } else {
                        Level[index] += layout.Layout[yj];
                    }
                }

                prevAbove[x] = layout.Id;
            }
        }

        // Height 18
        //  3 chars
        // Width 50 
        //  10 char

    }
}