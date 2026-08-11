using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CombatTrackerSystem : Singleton<CombatTrackerSystem>
{

    public int Round { get; private set; } = 0;

    private void OnEnable()
    {
        ActionSystem.SubscribeReaction<AfterPlayerTurnGA>(this, TurnStartReaction, ReactionTiming.PRE);
    }

    private void OnDisable()
    {
        ActionSystem.UnsubscribeReaction<AfterPlayerTurnGA>(this, TurnStartReaction, ReactionTiming.PRE);
    }

    private void TurnStartReaction(GameAction obj)
    {
        Round++;
    }
}
