using UnityEngine;

public class InSoakedLaneFilter : CombatantFilter
{
    protected override bool TargetIsValid(EffectContext context, CombatantView target)
    {
        return target.Slot.Lane.IsSoaked;
    }
}
