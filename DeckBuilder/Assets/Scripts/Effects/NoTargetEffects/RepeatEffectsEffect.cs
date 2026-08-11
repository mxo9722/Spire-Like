using SerializeReferenceEditor;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RepeatEffectsEffect : NoTargetEffect, IDynamicEffectText
{
    [SerializeReference, SR] private Quantity _repeatCount;
    [SerializeReference, SR] private AutoTargetEffect[] _autoTargetEffects;

    public override IDynamicEffectText[] GetDynamicTextEffects()
    {
        List<IDynamicEffectText> dynamicEffectTexts = new() { this };

        foreach(AutoTargetEffect autoTargetEffect in _autoTargetEffects)
        {
            dynamicEffectTexts.AddRange(autoTargetEffect.Effects.SelectMany(e => e.GetDynamicTextEffects()));
        }

        return dynamicEffectTexts.ToArray();
    }

    protected override GameAction GetGameAction(EffectContext context)
    {
        List<AutoTargetEffect> effects = new();
        int count = _repeatCount.GetAmount(context);

        for(int i = 0; i < count; i++)
        {
            effects.AddRange(_autoTargetEffects);
        }

        MultipleEffectsGA multipleEffectsGA = new(context, effects);
        return multipleEffectsGA;
    }

    public override AutoTargetEffect[] GetNestedEffects()
    {
        return _autoTargetEffects;
    }

    public string GetStaticText()
    {
        return _repeatCount.GetStaticAmount().ToString();
    }

    public string GetDynamicText(EffectContext context, List<CombatantView> targetCombatants = null, List<LaneView> targetLanes = null)
    {
        return _repeatCount.GetAmount(context).ToString();
    }
}
