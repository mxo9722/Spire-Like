using System.Collections.Generic;
using UnityEngine;

public class ExhaustCardGA : GameAction
{
    public List<Card> Cards { get; private set; }

    public ExhaustCardGA(Card card)
    {
        Cards = new() { card };
    }
    
    public ExhaustCardGA(List<Card> cards)
    {
        Cards = cards;
    }
}
