using Dungeon;
using Dungeon.Geneartion;
using Dungeon.Utils;
using Xunit.Abstractions;

namespace Tests.Geneartion;

public class CorridorTests {
    private FakeRandom fake = new FakeRandom();
    public CorridorTests(ITestOutputHelper output){
        Console.SetOut(new TestWriter(output));
    }

    [Fact]
    public void CreateNorthCorridor(){
        Corridor corridor = new Corridor(6,Direction.North);

        Assert.Equal(3,corridor.GetWidth());
        
        Assert.Equal(6,corridor.GetHeight());
    }
    [Fact]
    public void CreateEastCorridor(){
        Corridor corridor = new Corridor(6,Direction.East);

        Assert.Equal(6,corridor.GetWidth());
        
        Assert.Equal(3,corridor.GetHeight());
    }

    [Fact]
    public void CreateSouthCorridor(){
        Corridor corridor = new Corridor(6,Direction.South);

        Assert.Equal(3,corridor.GetWidth());
        
        Assert.Equal(6,corridor.GetHeight());
    }
    [Fact]
    public void CreateWestCorridor(){
        Corridor corridor = new Corridor(6,Direction.West);

        Assert.Equal(6,corridor.GetWidth());
        
        Assert.Equal(3,corridor.GetHeight());
    }

    [Fact]
    public void GetPointsForCorridorNorth(){
        Corridor corridor = new Corridor(6,Direction.North);
        IEnumerable<Vector2> points = corridor.GetCorridorPoints(10,10,fake);

        Assert.Equal(18,points.Count());

        Console.WriteLine("TEST");
        foreach(var item in points){
            Console.WriteLine($"{item}");
        }
    }
    [Fact]
    public void GetPointsForCorridorSouth(){
        Corridor corridor = new Corridor(6,Direction.South);
        IEnumerable<Vector2> points = corridor.GetCorridorPoints(10,10,fake);

        Assert.Equal(18,points.Count());

        Console.WriteLine("TEST");
        foreach(var item in points){
            Console.WriteLine($"{item}");
        }
    }

    [Fact]
    public void GetPointsForCorridorWest(){
        Corridor corridor = new Corridor(6,Direction.West);

        IEnumerable<Vector2> points = corridor.GetCorridorPoints(10,10,fake);

        Assert.Equal(18,points.Count());

   
        foreach(var item in points){
            Console.WriteLine($"{item}");
        }
    }

    [Fact]
    public void GetPointsForCorridorEast(){
        Corridor corridor = new Corridor(6,Direction.East);
        IEnumerable<Vector2> points = corridor.GetCorridorPoints(10,10,fake);

        Assert.Equal(18,points.Count());

        Console.WriteLine("TEST");
        foreach(var item in points){
            Console.WriteLine($"{item}");
        }
    }
}