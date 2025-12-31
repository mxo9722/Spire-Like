using SerializeReferenceEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StatusEffectAppliedSER : StatusEffectReaction
{

    [SerializeField] private StatusEffectType _targetType;
    [SerializeField] private bool _repeatPerStack = true;
    [SerializeReference, SR] private List<CombatantFilter> _combatantFilters;



    public override int SubConditionIsMet(CombatantView owner, GameAction gameAction)
    {
        if(gameAction is AddStatusEffectGA addStatusEffectGA)
        {
            int count = addStatusEffectGA.Targets.Count;

            if (_combatantFilters.Count > 0)
                count = addStatusEffectGA.Targets.ApplyFilters(_combatantFilters).Count();

            int trueVal = _repeatPerStack ? count : 1;
            bool success = addStatusEffectGA.StatusEffectType == _targetType;

            return success ? trueVal : 0;
        }

        return 0;
    }

    public override void SubscribeCondition(object subscriber, Action<GameAction> reaction)
    {
        ActionSystem.SubscribeReaction<AddStatusEffectGA>(subscriber, reaction, _reactionTiming);
    }

    public override void UnsubscribeCondition(object subscriber, Action<GameAction> reaction)
    {
        ActionSystem.UnsubscribeReaction<AddStatusEffectGA>(subscriber, reaction, _reactionTiming);
    }
}
