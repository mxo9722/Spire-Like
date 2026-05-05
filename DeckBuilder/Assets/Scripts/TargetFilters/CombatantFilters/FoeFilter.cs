using UnityEngine;

public class FoeFilter : CombatantFilter
{
    protected override bool TargetIsValid(EffectContext context, CombatantView target)
    {
        if (context == null || (context.Caster == null && context.PlayedCard == null))
            return false;

        if (context.Caster is HeroView && target is NPCView) return true;
        if (context.Caster is NPCView && target is HeroView) return true;

        return false;
    }
}
