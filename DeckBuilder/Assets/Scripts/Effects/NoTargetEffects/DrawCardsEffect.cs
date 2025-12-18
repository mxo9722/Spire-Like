using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DrawCardsEffect : NoTargetEffect
{
    [SerializeField] private int _drawAmount;

    protected override GameAction GetGameAction(EffectContext context)
    {
        DrawCardsGA drawCardGA = new DrawCardsGA(_drawAmount);
        return drawCardGA;
    }
}
