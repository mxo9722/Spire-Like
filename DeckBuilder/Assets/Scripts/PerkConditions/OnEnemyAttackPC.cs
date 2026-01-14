using System;
using UnityEngine;

public class OnEnemyAttackPC : PerkCondition
{
    public override bool SubConditionIsMet(GameAction gameAction)
    {
        return gameAction is AttackHeroGA attackHeroGA && attackHeroGA.Targets.IndexOf(HeroSystem.Instance.HeroView) != -1;
    }

    public override void SubscribeCondition(Action<GameAction> reaction)
    {
        ActionSystem.SubscribeReaction<AttackHeroGA>(this, reaction, reactionTiming);
    }

    public override void UnsubscribeCondition(Action<GameAction> reaction)
    {
        ActionSystem.UnsubscribeReaction<AttackHeroGA>(this,reaction, reactionTiming);

    }
}
