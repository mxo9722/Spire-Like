using UnityEngine;

public class ExhaustCardGA : GameAction
{
    public CardView CardView { get; private set; }

    public ExhaustCardGA(CardView cardView)
    {
        CardView = cardView;
    }
}
