using System.Collections.Generic;
using UnityEngine;

public class SetSoakGA : GameAction
{
    public List<LaneView> LaneViews { get; private set; }
    public bool Soaked { get; private set; }

    public SetSoakGA(List<LaneView> laneViews, bool soaked)
    {
        LaneViews = laneViews;
        Soaked = soaked;
    }
}
