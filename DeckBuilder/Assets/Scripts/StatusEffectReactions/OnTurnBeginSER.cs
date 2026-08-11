using System;
using UnityEngine;

public class OnTurnBeginSER : StatusEffectReaction
{
    public override int SubConditionIsMet(CombatantView owner, GameAction gameAction)
    {
        if (owner is HeroView && gameAction is BeforePlayerTurnGA)
        {
            return 1;
        }

        if(owner is NPCView && gameAction is NPCActGA npcActGA && npcActGA.NPC == owner)
        {
            return 1;
        }

        return 0;
    }

    public override void SubscribeCondition(object subscriber, Action<GameAction> reaction)
    {
        ActionSystem.SubscribeReaction<BeforePlayerTurnGA>(subscriber,reaction,_reactionTiming);
        ActionSystem.SubscribeReaction<NPCActGA>(subscriber,reaction,_reactionTiming);
    }

    public override void UnsubscribeCondition(object subscriber, Action<GameAction> reaction)
    {
        ActionSystem.UnsubscribeReaction<BeforePlayerTurnGA>(subscriber, reaction, _reactionTiming);
        ActionSystem.UnsubscribeReaction<NPCActGA>(subscriber, reaction, _reactionTiming);
    }
}
