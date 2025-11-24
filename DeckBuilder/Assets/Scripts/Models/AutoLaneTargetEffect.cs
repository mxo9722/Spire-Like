using SerializeReferenceEditor;
using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AutoLaneTargetEffect : AutoTargetEffect
{

    [field: SerializeReference, SR] public LaneTargetMode TargetMode { get; private set; }

    public override Effect Effect { get => _laneEffect; }


    [field: SerializeReference, SR] private LaneTargetEffect _laneEffect;

    public override GameAction GetGameAction(TargetModeContext targetModeContext)
    {
        List<LaneView> targets = TargetMode.GetTargets(targetModeContext);
        PerformEffectsGA performEffectsGA = new(Effect, targets);
        return performEffectsGA;
    }

    public override string GetDynamicText(TargetModeContext targetModeContext)
    {
        IDynamicEffectText dynamicEffectText = GetDynamicTextEffect();

        string value = dynamicEffectText.GetDynamicText(targetModeContext.Caster, targetLanes:TargetMode.GetTargets(targetModeContext));

        return value;
    }

    public override List<StatusEffectType> GetAllStatusEffects()
    {
        List<StatusEffectType> statusEffects = new();

        var oStatusEffects = TargetMode.GetAllStatusEffects();

        if(oStatusEffects != null)
            statusEffects.AddRange(oStatusEffects);

        oStatusEffects = _laneEffect.GetAllStatusEffects();

        if(oStatusEffects != null)
            statusEffects.AddRange(oStatusEffects);

        return statusEffects;
    }
}
