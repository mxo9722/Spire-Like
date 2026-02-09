using System;
using UnityEngine;

public class OnDiscardCRC : CardReactionCondition
{

    public override bool SubConditionIsMet(GameAction gameAction)
    {
        if (gameAction is DiscardCardGA discardCardGA) 
        {
            if (discardCardGA.Target == _owner)
            {
                return true;
            }
        }

        return false;
    }

    public override void SubscribeCondition(Action<GameAction> reaction)
    {
        ActionSystem.SubscribeReaction<DiscardCardGA>(_owner, reaction, reactionTiming);
    }

    public override void UnsubscribeCondition(Action<GameAction> reaction)
    {
        ActionSystem.UnsubscribeReaction<DiscardCardGA>(_owner, reaction, reactionTiming);
    }


    public override CardReactionCondition Clone()
    {
        OnDiscardCRC onDiscardCRC = new();

        onDiscardCRC.reactionTiming = reactionTiming;

        return onDiscardCRC;
    }
}
