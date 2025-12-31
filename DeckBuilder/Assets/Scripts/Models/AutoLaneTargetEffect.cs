using SerializeReferenceEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AutoLaneTargetEffect : AutoTargetEffect
{

    [field: SerializeReference, SR] public LaneTargetMode TargetMode { get; private set; }

    public override Effect Effect { get => _laneEffect; }


    [field: SerializeReference, SR] private LaneTargetEffect _laneEffect;

    public override GameAction GetGameAction(EffectContext context)
    {
        List<LaneView> targets = TargetMode.GetTargets(context);
        PerformEffectsGA performEffectsGA = new(context, Effect, targets);
        return performEffectsGA;
    }

    public override string GetDynamicText(EffectContext context)
    {
        IDynamicEffectText dynamicEffectText = GetDynamicTextEffect();

        string value = dynamicEffectText.GetDynamicText(context, targetLanes:TargetMode.GetTargets(context));

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

    public override bool RequiresUserInput()
    {
        return TargetMode is IUserInputTM;
    }

    public override IEnumerator WaitForUserInput()
    {
        if (TargetMode is IUserInputTM userInputTM)
            yield return userInputTM.WaitForUserInput();
    }
}
