using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Map
{
    [field: SerializeField] public List<Room> Rooms { get; private set; }

    public Map(List<Room> rooms)
    {
        Rooms = rooms;
    }
}
