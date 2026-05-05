using SerializeReferenceEditor;
using System.Collections.Generic;
using UnityEngine;

public class AddDamageToCardsEffect : CardTargetEffect, IDynamicEffectText
{
    [SerializeReference, SR] private Quantity _extraDamage = new SetQ(0);

    protected override GameAction GetGameAction(EffectContext context, List<Card> cardTargets)
    {
        ExtraAttackDamageCM extraAttackDamageCM = new(_extraDamage.GetAmount(context));
        AddCardModifierGA addCardModifierGA = new(cardTargets, extraAttackDamageCM);

        return addCardModifierGA;
    }

    public string GetDynamicText(EffectContext context, List<CombatantView> targetCombatants = null, List<LaneView> targetLanes = null)
    {
        return _extraDamage.GetAmount(context).ToString();
    }

    public string GetStaticText()
    {
        return _extraDamage.GetStaticAmount().ToString();
    }
}
