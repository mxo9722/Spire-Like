using SerializeReferenceEditor;
using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AutoCombatantTargetEffect : AutoTargetEffect
{

    [field: SerializeReference, SR] public CombatantTargetMode TargetMode { get; private set; }

    public override Effect Effect { get => _combatantEffect; }


    [field: SerializeReference, SR] private CombatantTargetEffect _combatantEffect;

    public override GameAction GetGameAction(TargetModeContext targetModeContext)
    {
        List<CombatantView> targets = TargetMode.GetTargets(targetModeContext);
        PerformEffectsGA performEffectsGA = new(_combatantEffect, targets);
        return performEffectsGA;
    }

    public override string GetDynamicText(TargetModeContext targetModeContext)
    {
        IDynamicEffectText dynamicEffectText = GetDynamicTextEffect();

        string value = dynamicEffectText.GetDynamicText(targetModeContext.Caster, targetCombatants:TargetMode.GetTargets(targetModeContext));

        return value;
    }

    public override List<StatusEffectType> GetAllStatusEffects()
    {
        List<StatusEffectType> statusEffects = new();

        var oStatusEffects = TargetMode.GetAllStatusEffects();

        if(oStatusEffects != null)
            statusEffects.AddRange(oStatusEffects);

        oStatusEffects = _combatantEffect.GetAllStatusEffects();

        if (oStatusEffects != null)
            statusEffects.AddRange(oStatusEffects);

        return statusEffects;
    }
}
