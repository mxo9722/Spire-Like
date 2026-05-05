using System.Collections.Generic;
using UnityEngine;

public class AddCardsToHandEffect : CardTargetEffect
{
    protected override GameAction GetGameAction(EffectContext context, List<Card> cardTargets)
    {
        AddCardsToHandGA addCardsToHandGA = new(cardTargets);
        return addCardsToHandGA;
    }
}
