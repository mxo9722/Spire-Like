using System;
using UnityEngine;

public class OnCombatStartPC : ReactionCondition
{
    public override bool SubConditionIsMet(GameAction gameAction)
    {
        return true;
    }

    public override void SubscribeCondition(Action<GameAction> reaction)
    {
        ActionSystem.SubscribeReaction<CombatStartGA>(this, reaction, reactionTiming);
    }

    public override void UnsubscribeCondition(Action<GameAction> reaction)
    {
        ActionSystem.UnsubscribeReaction<CombatStartGA>(this, reaction, reactionTiming);
    }
}
