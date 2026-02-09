using SerializeReferenceEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StatusEffectGainedByOwnerSER : StatusEffectReaction
{

    [SerializeField] private StatusEffect _targetType;
    [SerializeReference, SR] private List<CombatantFilter> _combatantFilters;

    public override int SubConditionIsMet(CombatantView owner, GameAction gameAction)
    {
        if(gameAction is AddStatusEffectGA addStatusEffectGA)
        {
            bool success = addStatusEffectGA.StatusEffectType == _targetType && addStatusEffectGA.StackCount > 0 && addStatusEffectGA.Targets.Contains(owner);

            if (_combatantFilters.Count > 0)
                success = success && _combatantFilters.TrueForAll(f => f.TestTarget( new(addStatusEffectGA.Caster), owner));

            return success ? 1 : 0;
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
