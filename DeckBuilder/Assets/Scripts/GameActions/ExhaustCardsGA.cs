using System.Collections.Generic;
using UnityEngine;

public class ExhaustCardsGA : GameAction
{
    public List<Card> Cards { get; private set; }

    public ExhaustCardsGA(Card card)
    {
        Cards = new() { card };
    }
    
    public ExhaustCardsGA(List<Card> cards)
    {
        Cards = cards;
    }
}
