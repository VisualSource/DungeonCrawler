using Dungeon;
using Dungeon.Geneartion;
using Dungeon.Utils;

namespace Tests.Geneartion;

public class RoomTests
{
    private static Room s_va_0061 = new Room() { 
        Id= "VA_0061", 
        Layout = new string[]{
            "             #####   ",
            "             #...#   ",
            "             #...#   ",
            "##############...#   ",
            "#................#   ",
            "#................#   ",
            "#................#   ",
            "####.............#   ",
            "   #.............#   ",
            "   #.............#   ",
            "   #.............#   ",
            "   #.............#   ",
            "   #.............#   ",
            "   #.............####",
            "   #................#",
            "   #................#",
            "   #................#",
            "   #...##############",
            "   #...#             ",
            "   #...#             ",
            "   #####             "
        } 
    };

    private static Room s_sp_0011 = new Room(){
        Id="SP_0011",
        Layout = new string[]{
            "#########",
            "#.......#",
            "#.......#",
            "#.......#",
            "#.......#",
            "##.....##",
            " #.....# ",
            " ##...## ",
            "  #...#  ",
            "  #...#  ",
            "  #...#  ",
            "  #...#  ",
            "  #...#  ",
            "  #...#  ",
            "  #...#  ",
            " ##...## ",
            " #.....# ",
            "##.....##",
            "#.......#",
            "#.......#",
            "#.......#",
            "#.......#",
            "#########",
        }
    };

    [Fact]
    public void TestGetHeight(){
        int height_0061 = s_va_0061.GetHeight();

        Assert.Equal(21,height_0061);

        int height_0011 = s_sp_0011.GetHeight();

        Assert.Equal(23,height_0011);
    }

    [Fact]
    public void TestGetWidth(){
        int width_0061 = s_va_0061.GetWidth();

        Assert.Equal(21,width_0061);

        int width_0011 = s_sp_0011.GetWidth();

        Assert.Equal(9,width_0011);
    }

    [Fact]
    public void GetStartingOffset_North_Single(){
        Vector2[] expectedRanges = new[]{
             new Vector2(4,6)
        };
        
        IEnumerable<Vector2> ranges = s_va_0061.GetStartingOffset(Direction.North);
        
        Assert.Single(ranges);

        Assert.Equal(expectedRanges,ranges);
    }
    [Fact]
    public void GetStartingOffset_South_Single(){
        Vector2[] expectedRanges = new Vector2[]{
            new Vector2(14,16)
        };
        IEnumerable<Vector2> ranges = s_va_0061.GetStartingOffset(Direction.South);

        Assert.Single(ranges);

        Assert.Equal(expectedRanges,ranges);
    }

    [Fact]
    public void GetStartingOffset_East_Single()
    {
        Vector2[] expectedRanges = new[]{ new Vector2(4,6) };
        IEnumerable<Vector2> ranges = s_va_0061.GetStartingOffset(Direction.East);

        Assert.Single(ranges);

        Assert.Equal(expectedRanges,ranges);
    }
    
    [Fact]
    public void GetStartingOffset_West_Single()
    {
        Vector2[] expectedRanges = new Vector2[]{
            new Vector2(14,16)
        };
        IEnumerable<Vector2> ranges = s_va_0061.GetStartingOffset(Direction.West);

        Assert.Single(ranges);

        Assert.Equal(expectedRanges,ranges);
    }

    [Fact]
    public void GetStartingOffset_East_Multi()
    {
        Vector2[] expectedRanges = new[]{
            new Vector2(1,4),
            new Vector2(18,21),
        };

        IEnumerable<Vector2> ranges = s_sp_0011.GetStartingOffset(Direction.East);

        Assert.Equal(2,ranges.Count());

        Assert.Equal(expectedRanges,ranges);
    }

    [Fact]
    public void GetStartingOffset_West_Multi()
    {
        Vector2[] expectedRanges = new[]{
            new Vector2(1,4),
            new Vector2(18,21),
        };

        IEnumerable<Vector2> ranges = s_sp_0011.GetStartingOffset(Direction.West);

        Assert.Equal(2,ranges.Count());

        Assert.Equal(expectedRanges,ranges);
    }
}