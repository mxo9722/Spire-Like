using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OnFoeLosesHealthGA : ReactionCondition
{

    [SerializeField] private string _dataKey;

    public override bool SubConditionIsMet(GameAction gameAction)
    {
        if(gameAction is DealDamageGA dealDamageGA)
        {
            return dealDamageGA.Targets.Any(t => t is NPCView npc && npc.IsEvil);
        }

        return false;
    }

    public override void SubscribeCondition(Action<GameAction> reaction)
    {
        ActionSystem.SubscribeReaction<DealDamageGA>(this, reaction, ReactionTiming.POST);
    }

    public override void UnsubscribeCondition(Action<GameAction> reaction)
    {
        ActionSystem.UnsubscribeReaction<DealDamageGA>(this, reaction, ReactionTiming.POST);
    }

    public override void SaveTargetData(EffectContext context, GameAction gameAction)
    {
        if (!string.IsNullOrEmpty(_dataKey) && gameAction is DealDamageGA dealDamageGA)
        {
            List<CombatantView> combatantViews = dealDamageGA.Targets.Where(t => t is NPCView npc && npc.IsEvil).ToList();
            context.AddData(_dataKey, combatantViews);
        }
    }
}
