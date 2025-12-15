using UnityEngine;

[System.Serializable]
public class CasterInLaneFilter : LaneFilter
{
    protected override bool TargetIsValid(EffectContext context, LaneView target)
    {
        return target.Contains(context.Caster);
    }
}
