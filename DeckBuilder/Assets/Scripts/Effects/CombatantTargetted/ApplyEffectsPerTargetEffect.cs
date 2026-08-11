using SerializeReferenceEditor;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ApplyEffectsPerTargetEffect : CombatantTargetEffect
{
    [SerializeReference, SR] private List<CombatantTargetEffect> _ctes;

    protected override GameAction GetGameAction(EffectContext context, List<CombatantView> combatantTargets)
    {
        List<AutoTargetEffect> actEffects = new();

        foreach(CombatantView unit in combatantTargets)
        {
            foreach(CombatantTargetEffect ctEffect in _ctes)
            {
                actEffects.Add(new AutoCombatantTargetEffect(new SpecificCTM(unit),ctEffect));
            }
        }

        MultipleEffectsGA multipleEffectsGA = new(context, actEffects);

        return multipleEffectsGA;
    }


    public override IDynamicEffectText[] GetDynamicTextEffects()
    {
        List<IDynamicEffectText> textEffect = new();

        foreach (CombatantTargetEffect effect in _ctes)
        {
            textEffect.AddRange(effect.GetDynamicTextEffects());
        }

        return textEffect.ToArray();
    }

}