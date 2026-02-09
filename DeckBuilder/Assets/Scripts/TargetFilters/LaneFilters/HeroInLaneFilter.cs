using UnityEngine;

public class HeroInLaneFilter : LaneFilter
{
    protected override bool TargetIsValid(EffectContext context, LaneView target)
    {
        return target.HeroView != null;
    }
}
