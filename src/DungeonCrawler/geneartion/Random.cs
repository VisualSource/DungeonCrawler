namespace Dungeon.Geneartion;

public interface IRandom {     
    public int Next();
    public int Next(int min, int max);
    public int Next(int maxValue);
}

public class FakeRandom : IRandom
{
    public int Next()
    {
        return 0;
    }

    public int Next(int min, int max)
    {
        return max;
    }

    public int Next(int maxValue)
    {
       return maxValue - 1;
    }
}

public class DefaultRandom: IRandom {
    private Random _rnd;
    public DefaultRandom(){
        _rnd = new Random();
    }
    public DefaultRandom(int seed) {
        _rnd = new Random(seed);
    }
    public int Next()
    {
        return _rnd.Next();
    }

    public int Next(int min, int max)
    {
        return _rnd.Next(min,max);
    }

    public int Next(int maxValue)
    {
        return _rnd.Next(maxValue);
    }
}