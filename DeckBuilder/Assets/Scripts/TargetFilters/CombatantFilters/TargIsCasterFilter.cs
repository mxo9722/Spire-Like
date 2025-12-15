using UnityEngine;

[System.Serializable]
public class TargIsCasterFilter : CombatantFilter
{
    protected override bool TargetIsValid(EffectContext context, CombatantView target)
    {
        return context.Caster == target;
    }
}
