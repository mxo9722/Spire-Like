using System.Collections.Generic;
using UnityEngine;

public class DealDamageEffect : CombatantTargetEffect, IDynamicEffectText
{
    [SerializeField] private int _damageAmount;

    protected override GameAction GetGameAction(CombatantView caster, List<CombatantView> combatantTargets)
    {
        DealDamageGA dealDamageGA = new DealDamageGA(_damageAmount, combatantTargets, caster);
        return dealDamageGA;
    }

    public string GetStaticText()
    {
        return _damageAmount.ToString();
    }

    public string GetDynamicText(CombatantView caster, List<CombatantView> targetCombatants = null, List<LaneView> targetLanes = null)
    {
        return DamageSystem.CardDamageTextFromAttack(_damageAmount, caster, targetCombatants);
    }
}
