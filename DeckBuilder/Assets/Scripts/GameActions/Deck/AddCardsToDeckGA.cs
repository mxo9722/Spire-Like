using System.Collections.Generic;
using UnityEngine;

public class AddCardsToDeckGA : GameAction
{
    public List<Card> Cards { get; private set; }

    public AddCardsToDeckGA(List<Card> cards)
    {
        Cards = cards;
    }
}
