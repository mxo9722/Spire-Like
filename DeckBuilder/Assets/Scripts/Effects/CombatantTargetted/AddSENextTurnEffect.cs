using SerializeReferenceEditor;
using System.Collections.Generic;
using UnityEngine;

public class AddSENextTurnEffect : CombatantTargetEffect
{
    [SerializeReference, SR] private SEIdentifier _statusEffect = new DictionaryEntrySEI();
    [SerializeReference, SR] private Quantity _amount = new SetQ();

    protected override GameAction GetGameAction(EffectContext context, List<CombatantView> combatantTargets)
    {
        NextTurnSEInfo nextTurnSEInfo = new(_statusEffect);

        AddStatusEffectGA addStatusEffectGA = new(nextTurnSEInfo, _amount.GetAmount(context), combatantTargets, context.Caster);

        return addStatusEffectGA;
    }
}
