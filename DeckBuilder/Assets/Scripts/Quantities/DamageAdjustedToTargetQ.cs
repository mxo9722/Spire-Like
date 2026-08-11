using SerializeReferenceEditor;
using System.Collections.Generic;
using UnityEngine;

public class DamageAdjustedToTargetQ : Quantity
{
    [SerializeReference, SR] private CombatantTargetMode _combatantTargetMode = new ManualTargetCTM();
    [SerializeReference, SR] private Quantity _baseDamage = new SetQ();

    public override int GetAmount(EffectContext context)
    {
        List<CombatantView> targets = _combatantTargetMode.GetTargets(context);
        int baseDamage = _baseDamage.GetAmount(context);

        return DamageSystem.GetDamageFromAttack(baseDamage, context, targets);
    }

    public override int GetStaticAmount()
    {
        return _baseDamage.GetStaticAmount();
    }
}
