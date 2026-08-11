using System;
using System.Linq;
using UnityEngine;

public class OnEnemyAttackPC : ReactionCondition
{
    public override bool SubConditionIsMet(GameAction gameAction)
    {
        return gameAction is AttackHeroGA attackHeroGA && attackHeroGA.Targets.Any(t => t is HeroView);
    }

    public override void SubscribeCondition(object subsccriber, Action<GameAction> reaction)
    {
        ActionSystem.SubscribeReaction<AttackHeroGA>(subsccriber, reaction, reactionTiming);
    }

    public override void UnsubscribeCondition(object subsccriber, Action<GameAction> reaction)
    {
        ActionSystem.UnsubscribeReaction<AttackHeroGA>(subsccriber, reaction, reactionTiming);

    }
}
