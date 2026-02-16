using System;
using UnityEngine;

public class OnOwnerDefeatedSER : StatusEffectReaction
{
    public override int SubConditionIsMet(CombatantView owner, GameAction gameAction)
    {
        if (gameAction is KillNpcGA killNPCGA && owner is NPCView npc)
        {
            return killNPCGA.NPCViews.Contains(npc) ? 1 : 0;
        }

        return 0;
    }

    public override void SubscribeCondition(object subscriber, Action<GameAction> reaction)
    {
        ActionSystem.SubscribeReaction<KillNpcGA>(subscriber, reaction, _reactionTiming);
    }

    public override void UnsubscribeCondition(object subscriber, Action<GameAction> reaction)
    {
        ActionSystem.UnsubscribeReaction<KillNpcGA>(subscriber, reaction, _reactionTiming);
    }
}
