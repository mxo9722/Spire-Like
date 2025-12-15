using UnityEngine;

public class DiscardAllCardsGA : GameAction
{
    [field: SerializeField] public bool ForEndOfTurn { get; private set; }

    public DiscardAllCardsGA(bool forEndOfTurn)
    {
        ForEndOfTurn = forEndOfTurn;
    }
}
