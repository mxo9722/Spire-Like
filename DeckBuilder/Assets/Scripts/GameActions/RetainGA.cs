using UnityEngine;

public class RetainGA : GameAction
{

    public Card card { get; private set; }

    public RetainGA(Card card)
    {
        this.card = card;
    }
}
