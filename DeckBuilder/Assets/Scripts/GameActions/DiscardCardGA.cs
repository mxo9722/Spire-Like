using UnityEngine;

public class DiscardCardGA : GameAction
{
    [field: SerializeField] public Card Target { get; private set; }
    [field: SerializeField] public CardView TargetView { get; private set; } = null;
    [field: SerializeField] public bool ForEndOfTurn { get; private set; }

    public DiscardCardGA(Card target, bool forEndOfTurn = false)
    {
        Target = target;
        ForEndOfTurn = forEndOfTurn;
    }
    
    public DiscardCardGA(CardView target, bool forEndOfTurn = false)
    {
        Target = target.Card;
        TargetView = target;
        ForEndOfTurn = forEndOfTurn;
    }

    public void SetCardView(CardView cardView)
    {
        TargetView = cardView;
    }
}
