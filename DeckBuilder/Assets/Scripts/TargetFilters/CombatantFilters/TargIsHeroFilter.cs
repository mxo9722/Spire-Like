using UnityEngine;

public class TargIsHeroFilter : CombatantFilter
{
    protected override bool TargetIsValid(EffectContext context, CombatantView target)
    {
        return target is HeroView;
    }
}
