using System;
using UnityEngine;

public class OnTargetCardPlayedSER : StatusEffectReaction
{
    public override int SubConditionIsMet(CombatantView owner, GameAction gameAction)
    {
        if(gameAction is PlayCardGA playCardGA)
        {
            return playCardGA.card.ManualTargetType != ManualTargetType.NONE ? 1 : 0;
        }

        return 0;
    }

    public override void SubscribeCondition(object subscriber, Action<GameAction> reaction)
    {
        ActionSystem.SubscribeReaction<PlayCardGA>(subscriber, reaction, _reactionTiming);
    }

    public override void UnsubscribeCondition(object subscriber, Action<GameAction> reaction)
    {
        ActionSystem.UnsubscribeReaction<PlayCardGA>(subscriber, reaction, _reactionTiming);
    }
}
