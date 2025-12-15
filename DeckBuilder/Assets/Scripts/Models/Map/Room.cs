using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public abstract class Room 
{
    [field: SerializeField] public bool IsCompleted { get; private set; } = false;
    [field: SerializeField] public int Seed { get; private set; }
    [field: SerializeField] public List<Room> PathedRooms { get; private set; } = new();
    [field: SerializeField] public List<Room> OriginRooms { get; private set; } = new();
    
    public int Level { get; private set; }
    public int Row { get; private set; }

    public abstract RoomType RoomType { get; }

    public Room(int level, int row, int seed = 0)
    {
        Level = level;
        Row = row;

        if (seed == 0)
            Seed = RNG.Random.Next();
        else
            Seed = seed;
    }

    public abstract void SetUp();

    public void SetCompleted()
    {
        IsCompleted = true;
    }

    public void AddConnection(Room room)
    {
        PathedRooms.Add(room);
    }
    
    public void AddOrigin(Room room)
    {
        OriginRooms.Add(room);
    }


    public bool IsSelectable()
    {
        if (OriginRooms.Count == 0)
            return true;

        return !IsCompleted && OriginRooms.Any(r => r.IsCompleted);
    }

    public bool IsChildOf(Room parent)
    {
        if (parent.PathedRooms.Count == 0)
            return false;
        if (parent.PathedRooms.Contains(this))
            return true;

        foreach(Room room in parent.PathedRooms)
        {
            if (IsChildOf(room))
                return true;
        }

        return false;
    }

    public abstract void Enter();
}
