using System.Collections.Generic;
using UnityEngine;

public class AddCardsToHandGA : GameAction
{
    public List<Card> Cards { get; private set; }

    public AddCardsToHandGA(List<Card> cards)
    {
        Cards = cards;
    }
}
