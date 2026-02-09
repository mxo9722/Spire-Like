using UnityEngine;

[System.Serializable]
public class TargIsCasterFilter : CombatantFilter
{
    protected override bool TargetIsValid(EffectContext context, CombatantView target)
    {
        if(context.Caster == null && context.PlayedCard != null)
        {
            return target is HeroView;
        }

        return context.Caster == target;
    }
}
