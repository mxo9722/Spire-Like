using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DrawCardsEffect : NoTargetEffect, IDynamicEffectText
{
    [SerializeField] private int _drawAmount;

    public string GetDynamicText(EffectContext context, List<CombatantView> targetCombatants = null, List<LaneView> targetLanes = null)
    {
        return _drawAmount.ToString();
    }

    public string GetStaticText()
    {
        return _drawAmount.ToString();
    }

    protected override GameAction GetGameAction(EffectContext context)
    {
        DrawCardsGA drawCardGA = new DrawCardsGA(_drawAmount);
        return drawCardGA;
    }
}
