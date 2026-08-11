using SerializeReferenceEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class AutoCombatantTargetEffect : AutoTargetEffect
{

    [field: SerializeReference, SR] public CombatantTargetMode TargetMode { get; private set; }

    public override Effect[] Effects { get => new Effect[] { _combatantEffect }; }
    public Effect Effect { get => _combatantEffect; }
    [field: SerializeReference, SR] private CombatantTargetEffect _combatantEffect;

    public AutoCombatantTargetEffect() { }

    public AutoCombatantTargetEffect(CombatantTargetMode targetMode, CombatantTargetEffect effect)
    {
        TargetMode = targetMode;
        _combatantEffect = effect;
    }

    public override GameAction GetGameAction(EffectContext context)
    {
        List<CombatantView> targets = TargetMode.GetTargets(context);
        PerformEffectsGA performEffectsGA = new(context,_combatantEffect, targets);
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
            List<CombatantView> targets = TargetMode.AllPossibleTargets(context, card);
            string value = dte.GetDynamicText(context, targets);
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
