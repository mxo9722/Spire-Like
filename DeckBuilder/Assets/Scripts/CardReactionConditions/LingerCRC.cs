using System;
using UnityEngine;

public class LingerCRC : CardReactionCondition
{

    public override bool SubConditionIsMet(GameAction gameAction)
    {
        if (gameAction is DiscardCardGA discardCardGA) 
        {
            if (discardCardGA.Target == _owner)
            {
                return discardCardGA.ForEndOfTurn;
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
        LingerCRC lingerCRC = new();

        lingerCRC.reactionTiming = reactionTiming;

        return lingerCRC;
    }
}
