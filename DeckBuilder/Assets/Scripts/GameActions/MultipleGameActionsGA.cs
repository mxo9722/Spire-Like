using System.Collections.Generic;
using UnityEngine;

public class MultipleGameActionsGA : GameAction
{
    public List<GameAction> GameActions;

    public MultipleGameActionsGA(List<GameAction> gameActions)
    {
        GameActions = gameActions;
    }
}
