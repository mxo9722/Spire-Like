using System;
using UnityEngine;

public class OnOwnerAttackSER : StatusEffectReaction
{

    [SerializeField] private string _unblockedDamageKey = "";

    public override int SubConditionIsMet(CombatantView owner, GameAction gameAction)
    {
        if (gameAction is DealDamageGA dealDamageGA)
        {
            if (dealDamageGA.IsAttack && dealDamageGA.Caster == owner)
                return 1;
        }

        return 0;
    }

    public override void SubscribeCondition(object subscriber, Action<GameAction> reaction)
    {
        ActionSystem.SubscribeReaction<DealDamageGA>(subscriber, reaction, _reactionTiming);
    }

    public override void UnsubscribeCondition(object subscriber, Action<GameAction> reaction)
    {
        ActionSystem.UnsubscribeReaction<DealDamageGA>(subscriber, reaction, _reactionTiming);
    }

    public override void SaveTargetData(EffectContext context, GameAction gameAction)
    {
        base.SaveTargetData(context, gameAction);

        if (gameAction is DealDamageGA dealDamageGA) 
        {
            if (!string.IsNullOrEmpty(_unblockedDamageKey))
                context.SetData(_unblockedDamageKey, dealDamageGA.UnblockedDamage);
        }
    }
}
