using SerializeReferenceEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AutoTargetlessEffect : AutoTargetEffect
{
    public override Effect Effect { get => _noTargetEffect; }
    [field: SerializeReference, SR] private NoTargetEffect _noTargetEffect;

    public override GameAction GetGameAction(EffectContext targetModeContext)
    {
        PerformEffectsGA performEffectsGA = new(Effect);
        return performEffectsGA;
    }

    public override string GetDynamicText(EffectContext targetModeContext)
    {
        return null;
    }

    public override List<StatusEffectType> GetAllStatusEffects()
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
