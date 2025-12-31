using SerializeReferenceEditor;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MultiAttackFoeEffect : CombatantTargetEffect
{
    [field: SerializeReference, SR] public Quantity AttackCount { get; private set; } = new SetQ();
    [field: SerializeReference, SR] public Quantity Damage { get; private set; } = new SetQ();


    [SerializeField] private string _unblockedKey = "";
    [SerializeField] private string _overkillKey = "";

    protected override GameAction GetGameAction(EffectContext context, List<CombatantView> combatantTargets)
    {
        MultiAttackHeroGA multiAttackHeroGA = new MultiAttackHeroGA(AttackCount.GetAmount(context), Damage.GetAmount(context), combatantTargets, context, _unblockedKey, _overkillKey);
        return multiAttackHeroGA;
    }
}
