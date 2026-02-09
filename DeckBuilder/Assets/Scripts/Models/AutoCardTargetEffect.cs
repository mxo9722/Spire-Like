using SerializeReferenceEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoCardTargetEffect : AutoTargetEffect
{
    [field: SerializeReference, SR] public CardTargetMode TargetMode { get; private set; }

    public override Effect[] Effects { get => new Effect[] { _cardEffect }; }

    [field: SerializeReference, SR] private CardTargetEffect _cardEffect;

    public override IDynamicEffectText[] GetDynamicTextEffects()
    {
        List<IDynamicEffectText> dets = new();

        dets.AddRange(TargetMode.GetDynamicTextEffects());

        foreach (Effect effect in Effects)
        {
            dets.AddRange(effect.GetDynamicTextEffects());
        }

        return dets.ToArray();
    }

    public override List<StatusEffect> GetAllStatusEffects()
    {
        List<StatusEffect> statusEffects = new();

        List<StatusEffect> oStatusEffects = TargetMode.GetAllStatusEffects();

        if (oStatusEffects != null)
            statusEffects.AddRange(oStatusEffects);

        oStatusEffects = _cardEffect.GetAllStatusEffects();

        if (oStatusEffects != null)
            statusEffects.AddRange(oStatusEffects);

        return statusEffects;
    }

    public override GameAction GetGameAction(EffectContext context)
    {
        List<Card> cards = TargetMode.GetTargets(context);
        PerformEffectsGA performEffectsGA = new(context, _cardEffect, cards);
        return performEffectsGA;
    }

    public override bool RequiresUserInput()
    {
        return TargetMode is IUserInputTM;
    }

    public override IEnumerator WaitForUserInput(EffectContext context)
    {
        if (TargetMode is IUserInputTM userInputTM)
            yield return userInputTM.WaitForUserInput(context);
    }

    public override NPCTargetTypes GetTargetIntent()
    {
        return TargetMode.GetTargetIntent();
    }
}
