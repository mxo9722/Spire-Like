using SerializeReferenceEditor;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MultiAttackFoeEffect : CombatantTargetEffect
{
    [field: SerializeReference, SR] public Quantity AttackCount { get; private set; } = new SetQ();
    [field: SerializeReference, SR] public Quantity Damage { get; private set; } = new SetQ();

    [field: SerializeField] public bool IndirectReduction = true;

    [SerializeField] private string _unblockedKey = "";
    [SerializeField] private string _overkillKey = "";
    [SerializeField] private string _onHitKey = "";
    [SerializeField] private string _hitCountKey = "";

    protected override GameAction GetGameAction(EffectContext context, List<CombatantView> combatantTargets)
    {
        MultiAttackHeroGA multiAttackHeroGA = new MultiAttackHeroGA(AttackCount.GetAmount(context), Damage.GetAmount(context), combatantTargets, context, IndirectReduction, _unblockedKey, _overkillKey, _onHitKey, _hitCountKey);
        return multiAttackHeroGA;
    }

    public int GetTotalDamage(EffectContext context, CombatantView target)
    {
        List<CombatantView> targets = null;

        if (target != null)
            targets = new() { target };

        int count = AttackCount.GetAmount(context);
        int damage = Damage.GetAmount(context);

        damage = DamageSystem.GetDamageFromAttack(damage, context, targets);

        //if (IndirectReduction)
        //    damage = EnemySystem.Instance.ApplyIndirectModifiers(damage, context.Caster, target);

        return damage * count;
    }
}
