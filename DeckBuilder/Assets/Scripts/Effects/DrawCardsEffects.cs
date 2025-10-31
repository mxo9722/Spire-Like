using System.Collections.Generic;
using UnityEngine;

public class DrawCardsEffects : Effect
{
    [SerializeField] private int _drawAmount;
    public override GameAction GetGameAction(List<CombatantView> targets, CombatantView caster)
    {
        DrawCardsGA drawCardGA = new DrawCardsGA(_drawAmount);
        return drawCardGA;
    }
}
