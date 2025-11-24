using SerializeReferenceEditor;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AutoTargetlessEffect : AutoTargetEffect
{
    public override Effect Effect { get => _noTargetEffect; }
    [field: SerializeReference, SR] private NoTargetEffect _noTargetEffect;

    public override GameAction GetGameAction(TargetModeContext targetModeContext)
    {
        PerformEffectsGA performEffectsGA = new(Effect);
        return performEffectsGA;
    }

    public override string GetDynamicText(TargetModeContext targetModeContext)
    {
        return null;
    }

    public override List<StatusEffectType> GetAllStatusEffects()
    {
        return _noTargetEffect.GetAllStatusEffects();
    }
}
