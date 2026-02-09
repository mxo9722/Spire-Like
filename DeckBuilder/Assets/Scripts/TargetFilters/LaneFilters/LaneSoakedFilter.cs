using UnityEngine;

public class LaneSoakedFilter : LaneFilter
{
    protected override bool TargetIsValid(EffectContext context, LaneView target)
    {
        return target.IsSoaked;
    }
}
