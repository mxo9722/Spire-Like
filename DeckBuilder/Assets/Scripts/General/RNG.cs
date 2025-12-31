using System;

public class RNG
{
    private static Random _random = null;
    private static Random _trivialRandom = null;
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

    public static Random TrivialRandom
    {
        get
        {
            if (_trivialRandom == null)
                _trivialRandom = new Random();
            return _trivialRandom;
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
    }

    public static void SetSeed(int seed)
    {
        _seed = seed;
        _random = new Random(Seed);
    }
}