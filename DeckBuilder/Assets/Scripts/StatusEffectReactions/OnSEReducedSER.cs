using System;
using UnityEngine;

public class OnSEReducedSER : StatusEffectReaction
{

    [SerializeField] private StatusEffectData _statusEffect;

    public override int SubConditionIsMet(CombatantView owner, GameAction gameAction)
    {
        if (gameAction is AddStatusEffectGA addStatusEffectGA)
        {
            if (_statusEffect.Info == addStatusEffectGA.StatusEffectInfo && addStatusEffectGA.StackCount < 0)
                return 1;
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
