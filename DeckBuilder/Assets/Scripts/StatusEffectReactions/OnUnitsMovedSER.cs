using SerializeReferenceEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OnUnitsMovedSER : StatusEffectReaction
{

    [SerializeReference, SR] private List<CombatantFilter> _filters;

    [SerializeReference, SR] private List<LaneFilter> _newLaneFilters;

    [SerializeField] private string _relevantCombatantsKey = "";

    public override int SubConditionIsMet(CombatantView owner, GameAction gameAction)
    {
        if(gameAction is MoveUnitsGA moveUnitsGA)
        {
            EffectContext context = new(owner);

            IEnumerable<CombatantView> validCombatants = moveUnitsGA.Moves.Keys;
            
            if(_filters.Count > 0)
                validCombatants = validCombatants.ToList().ApplyFilters(_filters, context);

            if (_newLaneFilters.Count > 0)
                validCombatants = validCombatants.Where(vc => _newLaneFilters.TrueForAll(lf => lf.TestTarget( context, moveUnitsGA.Moves[vc])));

            int count = validCombatants.Count();

            return count;
        }

        return 0;
    }

    public override void SaveTargetData(EffectContext context, GameAction gameAction)
    {
        if (gameAction is MoveUnitsGA moveUnitsGA)
        {
            IEnumerable<CombatantView> validCombatants = moveUnitsGA.Moves.Keys;

            if (_filters.Count > 0)
                validCombatants = validCombatants.ToList().ApplyFilters(_filters, context);

            if (_newLaneFilters.Count > 0)
                validCombatants = validCombatants.Where(vc => _newLaneFilters.TrueForAll(lf => lf.TestTarget(context, moveUnitsGA.Moves[vc])));

            int count = validCombatants.Count();

            if (count > 0 && !string.IsNullOrWhiteSpace(_relevantCombatantsKey))
                context.AddData(_relevantCombatantsKey, validCombatants.ToList());
        }
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
