using UnityEngine;

public abstract class CardReactionCondition : ReactionCondition
{
    protected Card _owner;

    public void SetUp(Card owner)
    {
        _owner = owner;
    }

    public abstract CardReactionCondition Clone();
}
