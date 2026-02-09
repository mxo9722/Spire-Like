using SerializeReferenceEditor;
using System;
using System.Collections.Generic;
using UnityEngine;

public class OnCardPlayedSER : StatusEffectReaction
{

    [SerializeReference, SR] private List<CardFilter> _filters;

    public override int SubConditionIsMet(CombatantView owner, GameAction gameAction)
    {
        if(gameAction is PlayCardGA playCardGA)
        {
            return _filters.Count == 0 || _filters.TrueForAll(f => f.TestTarget(playCardGA.GetEffectContext(), playCardGA.card)) ? 1 : 0;
        }

        return 0;
    }

    public override void SubscribeCondition(object subscriber, Action<GameAction> reaction)
    {
        ActionSystem.SubscribeReaction<PlayCardGA>(subscriber, reaction, _reactionTiming);
    }

    public override void UnsubscribeCondition(object subscriber, Action<GameAction> reaction)
    {
        ActionSystem.UnsubscribeReaction<PlayCardGA>(subscriber, reaction, _reactionTiming);
    }
}
