using UnityEngine;

public class DrawCardsGA : GameAction
{
    public int amount { get; set; }

    public DrawCardsGA(int amount)
    {
        this.amount = amount;
    }
}
