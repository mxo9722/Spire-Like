using System;
using UnityEngine;

public class OnAttackHeroSER : StatusEffectReaction
{
    public override int SubConditionIsMet(CombatantView owner, GameAction gameAction)
    {
        if(gameAction is AttackHeroGA attackHeroGA)
        {
            return attackHeroGA.Caster == owner ? 1 : 0;
        }

        return 0;
    }

    public override void SubscribeCondition(object subscriber, Action<GameAction> reaction)
    {
        ActionSystem.SubscribeReaction<AttackHeroGA>(subscriber, reaction, _reactionTiming);
    }

    public override void UnsubscribeCondition(object subscriber, Action<GameAction> reaction)
    {
        ActionSystem.UnsubscribeReaction<AttackHeroGA>(subscriber, reaction, _reactionTiming);
    }
}
