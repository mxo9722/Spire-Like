using SerializeReferenceEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AutoTargetlessEffect : AutoTargetEffect
{
    public override Effect[] Effects { get => new Effect[] { _noTargetEffect }; }
    [field: SerializeReference, SR] private NoTargetEffect _noTargetEffect;

    public override GameAction GetGameAction(EffectContext context)
    {
        PerformEffectsGA performEffectsGA = new(context, _noTargetEffect);
        return performEffectsGA;
    }

    public override List<StatusEffect> GetAllStatusEffects()
    {
        return _noTargetEffect.GetAllStatusEffects();
    }

    public override bool RequiresUserInput()
    {
        return false;
    }

    public override IEnumerator WaitForUserInput()
    {
        yield return null;
    }
}
