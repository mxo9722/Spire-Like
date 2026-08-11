using System;
using UnityEngine;

public abstract class CardReactionCondition : ReactionCondition
{
    protected Card _owner;

    public void SetUp(Card owner)
    {
        _owner = owner;
    }

    public abstract CardReactionCondition Clone();

    public override void SubscribeCondition(object subscriber, Action<GameAction> reaction) => SubscribeCondition(reaction);
    public override void UnsubscribeCondition(object subscriber, Action<GameAction> reaction) => UnsubscribeCondition(reaction);

    protected abstract void SubscribeCondition(Action<GameAction> reaction);
    protected abstract void UnsubscribeCondition(Action<GameAction> reaction);
}
