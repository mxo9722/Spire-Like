using System;

public class RNG
{
    private static Random _random = null;
    private static int _seed = 0;

    public static Random Random
    {
        get
        {
            if (_random == null)
                _random = new Random(Seed);
            return _random;
        }
    }

    public static int Seed
    {
        get
        {
            if(_seed == 0)
                _seed = Guid.NewGuid().GetHashCode();
            return _seed;
        }
        set
        {
            _seed = value;
            _random = new Random(_seed);
        }
    }
}
