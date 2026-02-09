using UnityEngine;

public class DrawCardGA : GameAction
{
    public bool ExpectAnotherDraw { get; private set; }
    public Card CardDrawn { get; private set; } = null;

    public DrawCardGA(bool expectAnotherDraw)
    {
        ExpectAnotherDraw = expectAnotherDraw;
    }

    public void SetCardDrawn(Card card)
    {
        CardDrawn = card;
    }
}
