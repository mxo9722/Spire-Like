using System.Collections.Generic;
using UnityEngine;

public class CycleGA : GameAction
{

    public List<Card> Cards { get; private set; }

    public CycleGA(List<Card> cards)
    {
        Cards = cards;
    }

}
