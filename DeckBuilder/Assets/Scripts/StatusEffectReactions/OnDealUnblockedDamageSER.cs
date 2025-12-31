using System;
using UnityEngine;

public class OnDealUnblockedDamageSER : StatusEffectReaction
{
    [SerializeField] private bool _repeatActionPerDamage = false;

    public override int SubConditionIsMet(CombatantView owner, GameAction gameAction)
    {
        int unblockedDamage = 0;



        if(_repeatActionPerDamage)
            return (unblockedDamage);
        return unblockedDamage > 0 ? 1 : 0;
    }

    public override void SubscribeCondition(object subscriber, Action<GameAction> reaction)
    {
        
    }

    public override void UnsubscribeCondition(object subscriber, Action<GameAction> reaction)
    {
        
    }
}
