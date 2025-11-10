using UnityEngine;

public class MoveHeroGA : GameAction
{
    public LaneView DestinationLane { get; private set; }
    public HeroView HeroView { get; private set; }

    public MoveHeroGA(LaneView destinationLane, HeroView heroView)
    {
        DestinationLane = destinationLane;
        HeroView = heroView;
    }
}
