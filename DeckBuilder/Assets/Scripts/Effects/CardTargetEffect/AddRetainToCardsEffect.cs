using System.Collections.Generic;
using UnityEngine;

public class AddRetainToCardsEffect : CardTargetEffect
{
    protected override GameAction GetGameAction(EffectContext context, List<Card> cardTargets)
    {
        RetainCM retainCM = new(true);
        AddCardModifierGA addCardModifierGA = new(cardTargets, retainCM);
        return addCardModifierGA;
    }
}
