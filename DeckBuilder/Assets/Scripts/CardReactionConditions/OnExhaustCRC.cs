using System;
using UnityEngine;

public class OnExhaustCRC : CardReactionCondition
{
    public override bool SubConditionIsMet(GameAction gameAction)
    {
        if (gameAction is ExhaustCardGA exhaustCardGA) 
        {
            return exhaustCardGA.Card == _owner;
        }

        return false;
    }

    protected override void SubscribeCondition(Action<GameAction> reaction)
    {
        ActionSystem.SubscribeReaction<ExhaustCardGA>(_owner, reaction, reactionTiming);
    }

    protected override void UnsubscribeCondition(Action<GameAction> reaction)
    {
        ActionSystem.UnsubscribeReaction<ExhaustCardGA>(_owner, reaction, reactionTiming);
    }


    public override CardReactionCondition Clone()
    {
        OnExhaustCRC onExhaustCRC = new();

        onExhaustCRC.reactionTiming = reactionTiming;

        return onExhaustCRC;
    }
}
