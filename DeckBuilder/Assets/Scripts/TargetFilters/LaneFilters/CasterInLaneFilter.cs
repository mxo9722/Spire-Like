using UnityEngine;

[System.Serializable]
public class CasterInLaneFilter : LaneFilter
{
    protected override bool TargetIsValid(EffectContext context, LaneView target)
    {
        if(context.Caster == null && context.PlayedCard != null)
        {
            return target.HeroView != null;
        }

        return target.Contains(context.Caster);
    }
}
