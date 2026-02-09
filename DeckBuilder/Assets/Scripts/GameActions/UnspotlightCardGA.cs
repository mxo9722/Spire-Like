using UnityEngine;

public class UnspotlightCardGA : GameAction
{
    public CardView Target { get; private set; }

    public UnspotlightCardGA(CardView target)
    {
        Target = target;
    }

}
