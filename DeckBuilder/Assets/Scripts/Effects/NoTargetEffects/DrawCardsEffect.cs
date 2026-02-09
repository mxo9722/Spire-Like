using SerializeReferenceEditor;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DrawCardsEffect : NoTargetEffect, IDynamicEffectText
{
    [SerializeReference, SR] private Quantity _drawAmount = new SetQ(1);

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
        int amount = _drawAmount.GetAmount(context);
        DrawCardsGA drawCardGA = new DrawCardsGA(amount);
        return drawCardGA;
    }
}
