using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XNode;

[Serializable]
public class EventRoom : Room
{
    public override RoomType RoomType => RoomType.EVENT;

    [field: SerializeField] public EventGraph EventGraph { get; private set; }
    [field: SerializeField] public Node CurrentNode { get; private set; } = null;

    public int EventIndex { get; private set; } = -1;

    public EventRoom(int level, int row, int seed = 0) : base(level, row, seed)
    {
        CurrentNode = EventGraph?.StartNode;
    }

    public override void Enter()
    {
        MapSystem.Instance.EnterEvent();
    }

    public override void SetUp()
    {
        List<int> skipList = OriginRooms.Where(n => n.RoomType == RoomType).Select(n => ((EventRoom)n).EventIndex).ToList();
        EventIndex = MapSystem.Instance.GetRoomIndex(skipList, RoomType);
        MapSystem.Instance.SetUpEventRoom(this, EventIndex);

        EventGraph.SetUp();

        if (CurrentNode == null)
            CurrentNode = EventGraph?.StartNode;
    }

    public void SetNode(Node node)
    {
        CurrentNode = node;
    }

    public void SetEventGraph(EventGraph eventGraph)
    {
        EventGraph = eventGraph;
    }
}
