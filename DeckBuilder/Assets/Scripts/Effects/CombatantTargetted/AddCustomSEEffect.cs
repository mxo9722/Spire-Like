using SerializeReferenceEditor;
using System.Collections.Generic;
using UnityEngine;

public class AddCustomSEEffect : CombatantTargetEffect
{
    [SerializeReference, SR] private SEIdentifier _statusEffect;
    [SerializeReference, SR] private Quantity _stackCount = new SetQ();
    [SerializeField] private bool _skipAnimation;

    protected override GameAction GetGameAction(EffectContext context, List<CombatantView> combatantTargets)
    {
        AddStatusEffectGA addStatusEffectGA = new(_statusEffect.GetSEInfo(), _stackCount.GetAmount(context), combatantTargets, context.Caster, _skipAnimation);
        return addStatusEffectGA;
    }
}
