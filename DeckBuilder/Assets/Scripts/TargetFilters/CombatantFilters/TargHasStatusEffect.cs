using UnityEngine;

public class TargHasStatusEffect : CombatantFilter
{
    [SerializeField] private StatusEffect _statusEffectType;

    protected override bool TargetIsValid(EffectContext context, CombatantView target)
    {
        if (target.GetAllActiveStatusEffects().Contains(_statusEffectType))
            return true;
        return false;
    }
}
