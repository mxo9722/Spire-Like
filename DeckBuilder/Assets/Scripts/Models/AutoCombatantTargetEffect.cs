using SerializeReferenceEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AutoCombatantTargetEffect : AutoTargetEffect
{

    [field: SerializeReference, SR] public CombatantTargetMode TargetMode { get; private set; }

    public override Effect Effect { get => _combatantEffect; }


    [field: SerializeReference, SR] private CombatantTargetEffect _combatantEffect;

    public override GameAction GetGameAction(EffectContext targetModeContext)
    {
        List<CombatantView> targets = TargetMode.GetTargets(targetModeContext);
        PerformEffectsGA performEffectsGA = new(_combatantEffect, targets);
        return performEffectsGA;
    }

    public override string GetDynamicText(EffectContext targetModeContext)
    {
        IDynamicEffectText dynamicEffectText = GetDynamicTextEffect();

        string value = dynamicEffectText.GetDynamicText(targetModeContext.Caster, targetCombatants:TargetMode.GetTargets(targetModeContext));

        return value;
    }

    public override List<StatusEffectType> GetAllStatusEffects()
    {
        List<StatusEffectType> statusEffects = new();

        List<StatusEffectType> oStatusEffects = TargetMode.GetAllStatusEffects();

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
}
