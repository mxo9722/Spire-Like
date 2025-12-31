using UnityEngine;

public class CasterMovedThisTurnCondition : Condition
{
    protected override bool IsConditionMet(EffectContext context)
    {
        return context.Caster.MovedThisRound;
    }
}
