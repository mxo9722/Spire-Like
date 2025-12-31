using System.Collections.Generic;
using UnityEngine;

public class AddCardsToDiscardEffect : CardTargetEffect
{
    protected override GameAction GetGameAction(CombatantView caster, List<Card> cardTargets)
    {
        return new AddCardsToDiscardGA(cardTargets);
    }
}
