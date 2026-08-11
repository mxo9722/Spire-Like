using SerializeReferenceEditor;
using System.Collections.Generic;
using UnityEngine;

public class TransferSEFromCasterEffect : CombatantTargetEffect
{
    [SerializeReference, SR] private SEIdentifier _statusEffect;
    [SerializeReference, SR] private Quantity _maxAmount;

    protected override GameAction GetGameAction(EffectContext context, List<CombatantView> combatantTargets)
    {
        TransferSE_GA transferSE_GA = new(context.Caster, combatantTargets, _statusEffect.GetSEInfo(), _maxAmount.GetAmount(context));

        return transferSE_GA;
    }
}
