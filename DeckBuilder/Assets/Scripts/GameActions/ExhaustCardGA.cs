using UnityEngine;

public class ExhaustCardGA : GameAction
{
    public Card Card { get; private set; }

    public ExhaustCardGA(Card card)
    {
        Card = card;
    }
}
