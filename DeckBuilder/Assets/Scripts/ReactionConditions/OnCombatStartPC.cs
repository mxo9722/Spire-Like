using System;
using UnityEngine;

public class OnCombatStartPC : ReactionCondition
{
    public override bool SubConditionIsMet(GameAction gameAction)
    {
        return true;
    }

    public override void SubscribeCondition(object subsccriber, Action<GameAction> reaction)
    {
        ActionSystem.SubscribeReaction<CombatStartGA>(subsccriber, reaction, reactionTiming);
    }

    public override void UnsubscribeCondition(object subsccriber, Action<GameAction> reaction)
    {
        ActionSystem.UnsubscribeReaction<CombatStartGA>(subsccriber, reaction, reactionTiming);
    }
}
