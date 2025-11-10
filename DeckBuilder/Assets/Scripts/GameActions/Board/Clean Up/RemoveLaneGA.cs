using UnityEngine;

public class RemoveLaneGA : GameAction
{
    public LaneView LaneView { get; private set; }

    public RemoveLaneGA(LaneView laneView)
    {
        LaneView = laneView;
    }
}
