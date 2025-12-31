using SerializeReferenceEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OnNPCKilledSER : StatusEffectReaction
{
    [SerializeReference, SR] private List<CombatantFilter> _filters;

    public override int SubConditionIsMet(CombatantView owner, GameAction gameAction)
    {
        if(gameAction is KillNpcGA killNpcGA)
        {
            if(_filters.Count > 0)
                return killNpcGA.NPCViews.Where(npc => _filters.TrueForAll(f => f.TestTarget(new(), npc))).Count();
            
            return killNpcGA.NPCViews.Count;
        }

        return 0;
    }

    public override void SubscribeCondition(object subscriber, Action<GameAction> reaction)
    {
        ActionSystem.SubscribeReaction<KillNpcGA>(subscriber, reaction, _reactionTiming);
    }

    public override void UnsubscribeCondition(object subscriber, Action<GameAction> reaction)
    {
        ActionSystem.UnsubscribeReaction<KillNpcGA>(subscriber, reaction, _reactionTiming);
    }
}
