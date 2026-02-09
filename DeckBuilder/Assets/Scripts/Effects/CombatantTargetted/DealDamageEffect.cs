using SerializeReferenceEditor;
using System.Collections.Generic;
using UnityEngine;

public class DealDamageEffect : CombatantTargetEffect, IDynamicEffectText
{
    [field: SerializeReference, SR] private Quantity _damage = new SetQ();

    protected override GameAction GetGameAction(EffectContext context, List<CombatantView> combatantTargets)
    {
        DealDamageGA dealDamageGA = new DealDamageGA(_damage.GetAmount(context), combatantTargets, context);
        return dealDamageGA;
    }

    public string GetStaticText()
    {
        return _damage.GetStaticAmount().ToString();
    }

    public string GetDynamicText(EffectContext context, List<CombatantView> targetCombatants = null, List<LaneView> targetLanes = null)
    {
        //if (context.Caster == null && context.PlayedCard != null)
        //{
        //    context = context.Clone();
        //    context.SetCaster(context.PlayedCard.GetOwnerView(context));
        //}

        return DamageSystem.CardDamageTextFromAttack(_damage.GetAmount(context), context, targetCombatants);
    }
}
