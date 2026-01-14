using SerializeReferenceEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OnUnitsMovedSER : StatusEffectReaction
{

    [SerializeReference, SR] private List<CombatantFilter> _filters;

    public override int SubConditionIsMet(CombatantView owner, GameAction gameAction)
    {
        if(gameAction is MoveUnitsGA moveUnitsGA)
        {
            EffectContext context = new(owner);
            int count = moveUnitsGA.Moves.Keys.ToList().ApplyFilters(_filters, context).Count();
            return count;
        }

        return 0;
    }

    public override void SubscribeCondition(object subscriber, Action<GameAction> reaction)
    {
        ActionSystem.SubscribeReaction<MoveUnitsGA>(subscriber, reaction, _reactionTiming);
    }

    public override void UnsubscribeCondition(object subscriber, Action<GameAction> reaction)
    {
        ActionSystem.UnsubscribeReaction<MoveUnitsGA>(subscriber, reaction, _reactionTiming);
    }
}
