using SerializeReferenceEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoCardTargetEffect : AutoTargetEffect
{
    [field: SerializeReference, SR] public CardTargetMode TargetMode { get; private set; }

    public override Effect Effect { get => _cardEffect; }

    [field: SerializeReference, SR] private CardTargetEffect _cardEffect;

    public override List<StatusEffectType> GetAllStatusEffects()
    {
        List<StatusEffectType> statusEffects = new();

        List<StatusEffectType> oStatusEffects = TargetMode.GetAllStatusEffects();

        if (oStatusEffects != null)
            statusEffects.AddRange(oStatusEffects);

        oStatusEffects = _cardEffect.GetAllStatusEffects();

        if (oStatusEffects != null)
            statusEffects.AddRange(oStatusEffects);

        return statusEffects;
    }

    public override string GetDynamicText(EffectContext targetModeContext)
    {
        IDynamicEffectText dynamicEffectText = GetDynamicTextEffect();

        string value = dynamicEffectText.GetDynamicText(targetModeContext.Caster);

        return value;
    }

    public override GameAction GetGameAction(EffectContext targetModeContext)
    {
        List<Card> cards = TargetMode.GetTargets(targetModeContext);
        PerformEffectsGA performEffectsGA = new(_cardEffect, cards);
        return performEffectsGA;
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
