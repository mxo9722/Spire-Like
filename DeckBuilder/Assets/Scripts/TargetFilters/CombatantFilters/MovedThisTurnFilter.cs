using UnityEngine;

public class MovedThisTurnFilter : CombatantFilter
{
    protected override bool TargetIsValid(EffectContext context, CombatantView target)
    {
        return target.GetCurrentRoundMovement() > 0;
    }
}
