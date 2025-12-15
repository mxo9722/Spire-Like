using UnityEngine;

public class DiscardCardGA : GameAction
{
    [field: SerializeField] public CardView Target { get; private set; }
    [field: SerializeField] public bool ForEndOfTurn { get; private set; }

    public DiscardCardGA(CardView target,bool forEndOfTurn)
    {
        Target = target;
        ForEndOfTurn = forEndOfTurn;
    }
}
