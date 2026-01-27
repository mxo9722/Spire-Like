using System.Collections.Generic;
using UnityEngine;

public class DiscardCardsGA : GameAction
{
    [field: SerializeField] public List<Card> Targets { get; private set; }
    [field: SerializeField] public bool ForEndOfTurn { get; private set; }

    public DiscardCardsGA(List<Card> targets, bool forEndOfTurn)
    {
        Targets = targets;
        ForEndOfTurn = forEndOfTurn;
    }
}
