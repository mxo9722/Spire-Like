using System.Collections.Generic;
using UnityEngine;

public class AddCardsToDiscardGA : GameAction
{
    public List<Card> Cards { get; private set; }

    public AddCardsToDiscardGA(List<Card> cards)
    {
        Cards = cards;
    }
}
