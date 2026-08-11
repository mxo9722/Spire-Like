using System;
using System.Collections.Generic;
using UnityEngine;

public class OnOwnerAttackedSER : StatusEffectReaction
{
    [SerializeField] private string _attackerKey;

    public override int SubConditionIsMet(CombatantView owner, GameAction gameAction)
    {
        if (gameAction is DealDamageGA dealDamageGA)
        {
            if (dealDamageGA.IsAttack && dealDamageGA.Targets.Contains(owner))
                return 1;
        }

        return 0;
    }

    public override void SubscribeCondition(object subscriber, Action<GameAction> reaction)
    {
        ActionSystem.SubscribeReaction<DealDamageGA>(subscriber, reaction, _reactionTiming);
    }

    public override void UnsubscribeCondition(object subscriber, Action<GameAction> reaction)
    {
        ActionSystem.UnsubscribeReaction<DealDamageGA>(subscriber, reaction, _reactionTiming);
    }

    public override void SaveTargetData(EffectContext context, GameAction gameAction)
    {
        if (!string.IsNullOrEmpty(_attackerKey) && gameAction is DealDamageGA dealDamageGA)
            context.SetData(_attackerKey, new List<CombatantView>() { dealDamageGA.Caster });
    }
}
