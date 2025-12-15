using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DrawCardsEffect : NoTargetEffect
{
    [SerializeField] private int _drawAmount;

    protected override GameAction GetGameAction(CombatantView caster)
    {
        DrawCardsGA drawCardGA = new DrawCardsGA(_drawAmount);
        return drawCardGA;
    }
}
