using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapViewCreator : Singleton<MapViewCreator>
{

    public int Levels => MapSystem.Instance.Levels;
    public int Rows => MapSystem.Instance.Rows;
    public int StartingRooms => MapSystem.Instance.StartingRooms;

    [SerializeField, SerializedDictionary("Room Type", "Data")]
    private SerializedDictionary<RoomType, RoomTypeData> _roomViewData;

    public Map GenerateMap()
    {
        System.Random random = RNG.Random;
        List<MapNode>[] nodes = new List<MapNode>[Levels];

        for(int x = 0; x < nodes.Length; x++)
        {
            List<MapNode> level = new List<MapNode>();
            nodes[x] = level;

            for(int y = 0; y < Rows; y++)
            {
                level.Add(new(x, y, random.Next()));
            }
        }

        List<MapNode> startLevel = nodes[0];

        //Generate paths from starting rooms
        for(int i = 0; i < 2; i++)
        {
            MapNode[] unusedRooms = startLevel.Where(r => r.PathToNodes == null).ToArray();

            int row = random.Next(unusedRooms.Length);

            GeneratePath(random, unusedRooms[row], nodes);
            GeneratePath(random, unusedRooms[row], nodes);
        }
        
        //generate paths from rooms that have already been pathed
        for(int i = 0; i < StartingRooms - 2; i++)
        {
            MapNode[] startRooms = startLevel.ToArray();

            int row = random.Next(startRooms.Length);

            GeneratePath(random, startRooms[row], nodes);
            GeneratePath(random, startRooms[row], nodes);
        }

        //TODO: assign types
        AssignLevelsType(nodes, RoomType.FIGHT, 0);
        AssignLevelsType(nodes, RoomType.TREASURE, Levels / 2);
        AssignLevelsType(nodes, RoomType.SAFE_HOUSE, Levels - 1);

        for (int x = 0; x < Levels; x++)
        {
            List<MapNode> level = nodes[x];
            foreach (MapNode mapNode in level)
            {
                AssignType(mapNode);
            }
        }

        HashSet<Room> rooms = new();

        foreach (List<MapNode> level in nodes)
        {
            foreach(MapNode mapNode in level)
            {
                if (mapNode.IsUtilized)
                {
                    Room room = mapNode.GetRoom();
                    rooms.Add(room);
                    room.SetUp();
                }
            }
        }

        Map map = new(rooms.ToList());

        return map;
    }

    private void GeneratePath(System.Random random, MapNode startPath, List<MapNode>[] nodes)
    {
        while (startPath != null)
            startPath = startPath.GeneratePath(random, nodes, Levels, Rows);
    }

    private void AssignType(MapNode mapNode)
    {
        if (mapNode.RoomType != RoomType.UNDECIDED || !mapNode.IsUtilized)
            return;

        List<RoomType> roomTypes = Enum.GetValues(typeof(RoomType)).Cast<RoomType>().ToList();

        float totalWeight = 0;

        for (int i = 0; i < roomTypes.Count; i++)
        {
            RoomType type = roomTypes[i];
            RoomTypeData data = _roomViewData[type];

            bool keep = data.Weight > 0 && mapNode.Level >= data.LowestLevel;

            if (!data.RepeatsAllowed && keep)
            {
                if (mapNode.PathFromNodes != null)
                {
                    keep = keep && !mapNode.PathFromNodes.Any(n => n.RoomType == type);

                    keep = keep && !mapNode.PathFromNodes.Any(m => m.PathToNodes.Any(n => n.RoomType == type && mapNode != n));
                }

                if (mapNode.PathToNodes != null)
                {
                    keep = keep && !mapNode.PathToNodes.Any(n => n.RoomType == type);
                }
            }

            if (keep)
                totalWeight += data.Weight;
            else
            {
                roomTypes.Remove(type);
                i--;
            }
        }

        float roll = (float)(RNG.Random.NextDouble() * totalWeight);
        for (int i = 0; i < roomTypes.Count; i++)
        {
            RoomType type = roomTypes[i];

            RoomTypeData data = _roomViewData[type];
            if(data.Weight >= roll)
            {
                mapNode.SetRoomType(type);
                break;
            }
            else
            {
                roll -= data.Weight;
            }
        }
    }
    
    private void AssignLevelsType(List<MapNode>[] nodes, RoomType roomType, int level)
    {
        foreach (MapNode mapNode in nodes[level])
            mapNode.SetRoomType(roomType);
    }
}
