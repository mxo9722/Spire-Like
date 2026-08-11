using SerializeReferenceEditor;
using System.Collections.Generic;
using UnityEngine;

public class ReduceCardsCostEffect : CardTargetEffect, IDynamicEffectText
{
    [SerializeReference, SR] private Quantity _costReduction = new SetQ(0);

    protected override GameAction GetGameAction(EffectContext context, List<Card> cardTargets)
    {
        ReduceCostCM reduceCostCM = new(_costReduction.GetAmount(context));
        AddCardModifierGA addCardModifierGA = new(cardTargets, reduceCostCM);

        return addCardModifierGA;
    }

    public string GetDynamicText(EffectContext context, List<CombatantView> targetCombatants = null, List<LaneView> targetLanes = null)
    {
        return _costReduction.GetAmount(context).ToString();
    }

    public string GetStaticText()
    {
        return _costReduction.GetStaticAmount().ToString();
    }
}
