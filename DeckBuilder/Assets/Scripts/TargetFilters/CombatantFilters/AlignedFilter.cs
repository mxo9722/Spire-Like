using UnityEngine;

public class AlignedFilter : CombatantFilter
{
    protected override bool TargetIsValid(EffectContext context, CombatantView target)
    {
        if (context == null || context.Caster == null)
            return false;

        return context.Caster.GetLaneDistance(target) == 0;
    }
}
