using SerializeReferenceEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class AutoCombatantTargetEffect : AutoTargetEffect
{

    [field: SerializeReference, SR] public CombatantTargetMode TargetMode { get; private set; }

    public override Effect[] Effects { get => new Effect[] { _combatantEffect }; }
    public Effect Effect { get => _combatantEffect; }
    [field: SerializeReference, SR] private CombatantTargetEffect _combatantEffect;

    public override GameAction GetGameAction(EffectContext context)
    {
        List<CombatantView> targets = TargetMode.GetTargets(context);
        PerformEffectsGA performEffectsGA = new(context,_combatantEffect, targets);
        return performEffectsGA;
    }

    public override List<StatusEffect> GetAllStatusEffects()
    {
        List<StatusEffect> statusEffects = new();

        List<StatusEffect> oStatusEffects = TargetMode.GetAllStatusEffects();

        if(oStatusEffects != null)
            statusEffects.AddRange(oStatusEffects);

        oStatusEffects = _combatantEffect.GetAllStatusEffects();

        if (oStatusEffects != null)
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
}
