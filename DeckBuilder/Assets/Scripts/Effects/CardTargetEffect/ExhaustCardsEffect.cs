using System.Collections.Generic;
using UnityEngine;

public class ExhaustCardsEffect : CardTargetEffect
{
    protected override GameAction GetGameAction(EffectContext context, List<Card> cardTargets)
    {
        ExhaustCardGA exhaustCardGA = new(cardTargets);

        return exhaustCardGA;
    }
}
