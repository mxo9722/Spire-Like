using System;
using UnityEngine;

public class OnFirstAttackedEachRoundSER : StatusEffectReaction
{
    public override int SubConditionIsMet(CombatantView owner, GameAction gameAction)
    {
        if(gameAction is DealDamageGA dealDamageGA)
        {
            if (!dealDamageGA.IsAttack) return 0;
            if (!dealDamageGA.Targets.Contains(owner)) return 0;
            if (_reactionTiming == ReactionTiming.PRE && owner.GetCurrentRoundAttacked() > 0) return 0;
            if (_reactionTiming == ReactionTiming.POST && owner.GetCurrentRoundAttacked() > 1) return 0;
            
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
}
