using SerializeReferenceEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AutoLaneTargetEffect : AutoTargetEffect
{

    [field: SerializeReference, SR] public LaneTargetMode TargetMode { get; private set; }

    public override Effect[] Effects { get => new Effect[] { _laneEffect }; }


    [field: SerializeReference, SR] private LaneTargetEffect _laneEffect;

    public override GameAction GetGameAction(EffectContext context)
    {
        List<LaneView> targets = TargetMode.GetTargets(context);
        PerformEffectsGA performEffectsGA = new(context, _laneEffect, targets);
        return performEffectsGA;
    }

    public override bool RequiresUserInput()
    {
        return TargetMode is INeedsUserInput;
    }

    public override IEnumerator WaitForUserInput(EffectContext context)
    {
        if (TargetMode is INeedsUserInput userInputTM)
            yield return userInputTM.WaitForUserInput(context);
    }

    public override string ApplyDynamicTextEffect(string description, int startIndex, EffectContext context, Card card)
    {
        IDynamicEffectText[] dtes = GetDynamicTextEffects();

        foreach (IDynamicEffectText dte in dtes)
        {
            List<LaneView> targets = TargetMode.AllPossibleTargets(context, card);
            string value = dte.GetDynamicText(context);
            description = description.Replace("{v" + (startIndex++) + "}", value);
        }

        return description;
    }

    public override NPCTargetTypes GetTargetIntent()
    {
        return TargetMode.GetTargetIntent();
    }

    public override void SimulatedPerform(EffectContext context)
    {
        if (GetGameAction(context) is SimulatedGameAction simulatedGameAction)
            simulatedGameAction.SimulatedPerform(context);
    }
}
