using SerializeReferenceEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StatusEffectAppliedPC : ReactionCondition
{

    [SerializeField] private StatusEffect _targetType;
    [SerializeReference, SR] private List<CombatantFilter> _combatantFilters;

    [SerializeField] private string _targetsDataKey = "";

    public override void SaveTargetData(EffectContext context, GameAction gameAction)
    {
        if (gameAction is AddStatusEffectGA addStatusEffectGA)
        {
            if (!string.IsNullOrEmpty(_targetsDataKey))
                context.SetData(_targetsDataKey, addStatusEffectGA.Targets);
        }
    }

    public override bool SubConditionIsMet(GameAction gameAction)
    {
        if (gameAction is AddStatusEffectGA addStatusEffectGA)
        {
            int count = addStatusEffectGA.Targets.Count;

            if (_combatantFilters.Count > 0)
                count = addStatusEffectGA.Targets.ApplyFilters(_combatantFilters).Count();

            bool success = addStatusEffectGA.StatusEffectInfo.EnumKey == _targetType;

            return success;
        }

        return false;
    }

    public override void SubscribeCondition(Action<GameAction> reaction)
    {
        ActionSystem.SubscribeReaction<AddStatusEffectGA>(this, reaction, reactionTiming);
    }

    public override void UnsubscribeCondition(Action<GameAction> reaction)
    {
        ActionSystem.UnsubscribeReaction<AddStatusEffectGA>(this, reaction, reactionTiming);
    }
}
