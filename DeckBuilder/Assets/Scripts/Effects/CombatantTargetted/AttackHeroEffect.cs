using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AttackHeroEffect : CombatantTargetEffect, IDynamicEffectText
{
    [field: SerializeField, Min(0)] public int Damage { get; private set; } = 0;

    [SerializeField] private string _unblockedKey = ""; 
    [SerializeField] private string _overkillKey = ""; 
    [SerializeField] private string _onHit = ""; 

    protected override GameAction GetGameAction(EffectContext context, List<CombatantView> combatantTargets)
    {
        if (combatantTargets.Count == 0)
            return null;

        AttackHeroGA attackHeroGA = new AttackHeroGA(Damage, combatantTargets, context, _unblockedKey, _overkillKey, _onHit);

        return attackHeroGA;
    }

    public string GetStaticText()
    {
        return Damage.ToString();
    }

    public string GetDynamicText(EffectContext context, List<CombatantView> targetCombatants = null, List<LaneView> targetLanes = null)
    {
        return DamageSystem.CardDamageTextFromAttack(Damage, context, targetCombatants);
    }
}
