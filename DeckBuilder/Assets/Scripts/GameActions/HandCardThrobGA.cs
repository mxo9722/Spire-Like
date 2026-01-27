using UnityEngine;

public class HandCardThrobGA : GameAction
{
    public Card Card { get; private set; }

    public HandCardThrobGA(Card card)
    {
        Card = card;
    }
}
