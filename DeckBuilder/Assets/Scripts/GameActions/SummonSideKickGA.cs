using UnityEngine;

public class SummonSideKickGA : GameAction
{
    public LaneView TargetLane { get; private set; }
    public NPCData Data { get; private set; }

    public SummonSideKickGA(LaneView targetLane, NPCData data)
    {
        TargetLane = targetLane;
        Data = data;
    }
}
