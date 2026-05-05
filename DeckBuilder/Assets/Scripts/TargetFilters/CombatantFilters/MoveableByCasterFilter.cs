using UnityEngine;

public class MoveableByCasterFilter : CombatantFilter
{
    protected override bool TargetIsValid(EffectContext context, CombatantView target)
    {
        CombatantView caster = context.Caster;

        if (caster == null)
        {
            if (context.PlayedCard == null)
                return true;

            caster = context.PlayedCard.GetOwnerView(context);

            if (caster == null)
                return true;
        }

        if (target == caster)
            return target.GetStatusEffectStacks(StatusEffect.PINNED) == 0;

        return target.GetStatusEffectStacks(StatusEffect.ANCHORED) == 0;
    }
}
