using UnityEngine;

public class CasterHasStatusEffectCondition : Condition
{
    [SerializeField] private StatusEffectType _statusEffectType;

    protected override bool IsConditionMet(EffectContext context)
    {
        var caster = context.Caster;

        if (caster == null)
            return false;

        return caster.GetStatusEffectStacks(_statusEffectType) > 0;
    }
}
