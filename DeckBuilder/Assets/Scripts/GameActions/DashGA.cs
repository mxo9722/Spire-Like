using UnityEngine;

public class DashGA : GameAction
{
    public HeroView HeroView { get; private set; }
    public LaneView Destination { get; private set; }

    public DashGA(HeroView heroView, LaneView destination)
    {
        HeroView = heroView;
        Destination = destination;
    }
}
