using System.Collections.Generic;
using UnityEngine;

public class AddCardModifierGA : GameAction
{
    public List<Card> Targets { get; private set; }
    public CardModifier CardModifier { get; private set; }

    public AddCardModifierGA(List<Card> targets, CardModifier modifier)
    {
        Targets = targets;
        CardModifier = modifier;
    }
}
