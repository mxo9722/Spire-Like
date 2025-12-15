using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapNode
{
    public List<MapNode> PathToNodes { get; private set; } = null;
    public List<MapNode> PathFromNodes { get; private set; } = null;

    public int Level { get; private set; }
    public int Row { get; private set; }
    public RoomType RoomType { get; private set; } = RoomType.UNDECIDED;
    public bool IsUtilized { get; private set; } = false;

    private int _seed;
    private Room _room = null;

    public MapNode(int level, int row, int seed)
    {
        Level = level;
        Row = row;
        _seed = seed;
    }

    public MapNode GeneratePath(System.Random random, List<MapNode>[] nodes, int levels, int rows)
    {
        IsUtilized = true;

        if (Level == levels - 1)
            return null;

        if (PathToNodes != null && PathToNodes.Count >= 2)
            return PathToNodes[random.Next(2)];

        int min = -1;
        int max = 1;

        int minRow = (Row + min + rows) % rows;
        int maxRow = (Row + max + rows) % rows;

        MapNode forwardNode = nodes[Level + 1][Row];

        if (forwardNode.PathFromNodes != null)
        {
            if (min == -1 && forwardNode.PathFromNodes.Contains(nodes[Level][minRow]))
                min = 0;
            if (max == 1 && forwardNode.PathFromNodes.Contains(nodes[Level][maxRow]))
                max = 0;
        }

        int direction = random.Next(min, max + 1);
        int pathRow = (Row + direction + rows) % rows;

        while (Level == levels - 2 && pathRow % 2 == 0)
        {
            direction = random.Next(min, max + 1);
            pathRow = (Row + direction + rows) % rows;
        }

        MapNode pathNode = nodes[Level + 1][pathRow];

        if (PathToNodes == null)
            PathToNodes = new();

        if(!PathToNodes.Contains(pathNode))
            PathToNodes.Add(pathNode);

        pathNode.AddPathFrom(this);

        return pathNode;
    }

    public Room GetRoom()
    {
        if (_room == null)
        {
            //TODO: switch statement to make the right room type
            switch (RoomType)
            {
                case RoomType.EVENT:
                    _room = new EventRoom(Level, Row, _seed);
                    break;
                case RoomType.FIGHT:
                    _room = new CombatRoom(Level, Row, _seed);
                    break;
                case RoomType.SAFE_HOUSE:
                    _room = new SafeHouseRoom(Level, Row, _seed);
                    break;
                case RoomType.BOSS_FIGHT:
                    _room = new BossRoom(Level, Row, _seed);
                    break;
                case RoomType.TREASURE:
                    _room = new TreasureRoom(Level, Row, _seed);
                    break;
                default:
                    _room = new TestRoom(Level, Row, _seed, RoomType);
                    break;
            }

            if (PathToNodes != null)
                foreach (MapNode mapNode in PathToNodes)
                {
                    Room room = mapNode.GetRoom();
                    _room.AddConnection(room);
                    room.AddOrigin(_room);
                }
        }

        return _room;
    }

    public void AddPathFrom(MapNode mapNode)
    {
        if (PathFromNodes == null)
            PathFromNodes = new();

        if (!PathFromNodes.Contains(mapNode))
            PathFromNodes.Add(mapNode);
    }

    public void SetRoomType(RoomType type)
    {
        RoomType = type;
    }
}
