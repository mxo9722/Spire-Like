using System.Collections.Generic;
using UnityEngine;

public class AddCardsToDeckEffect : CardTargetEffect
{
    protected override GameAction GetGameAction(CombatantView caster, List<Card> cardTargets)
    {
        AddCardsToDeckGA addCardsToDeckGA = new(cardTargets);
        return addCardsToDeckGA;
    }
}
