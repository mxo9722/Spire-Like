using System;
using UnityEngine;

[Serializable]
public abstract class Room 
{
    [field: SerializeField] public bool IsCompleted { get; private set; } = false;
    [field: SerializeField] public int Seed { get; private set; }

    public Room(int seed = 0)
    {
        if (seed == 0)
            Seed = RNG.Random.Next();
        else
            Seed = seed;
    }

    public void SetCompleted()
    {
        IsCompleted = true;
    }
}
