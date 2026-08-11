using SerializeReferenceEditor;
using System.Collections.Generic;
using UnityEngine;

public class AddBlockToCardsEffect : CardTargetEffect, IDynamicEffectText
{
    [SerializeReference, SR] private Quantity _extraBlock = new SetQ(0);

    protected override GameAction GetGameAction(EffectContext context, List<Card> cardTargets)
    {
        ExtraBlockCM extraBlockDamageCM = new(_extraBlock.GetAmount(context));
        AddCardModifierGA addCardModifierGA = new(cardTargets, extraBlockDamageCM);

        return addCardModifierGA;
    }

    public string GetDynamicText(EffectContext context, List<CombatantView> targetCombatants = null, List<LaneView> targetLanes = null)
    {
        return _extraBlock.GetAmount(context).ToString();
    }

    public string GetStaticText()
    {
        return _extraBlock.GetStaticAmount().ToString();
    }
}