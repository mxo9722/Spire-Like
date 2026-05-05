using SerializeReferenceEditor;
using System.Collections.Generic;
using UnityEngine;

public class MultiplySEEffect : CombatantTargetEffect
{
    [SerializeReference, SR] private SEIdentifier _statusEffect;
    [SerializeField] private float _multiplier = 2;

    protected override GameAction GetGameAction(EffectContext context, List<CombatantView> combatantTargets)
    {
        MultiplyStatusEffectGA multiplyStatusEffectGA = new(_statusEffect.GetSEInfo(), _multiplier, combatantTargets);
        return multiplyStatusEffectGA;
    }
}
