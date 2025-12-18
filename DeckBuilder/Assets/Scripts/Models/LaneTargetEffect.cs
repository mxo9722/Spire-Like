using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class LaneTargetEffect : Effect
{
    public override GameAction GetGameAction(EffectContext context, List<CombatantView> combatantTargets = null, List<LaneView> laneTargets = null, List<Card> cardTargets = null)
    {
        return GetGameAction(context, laneTargets);
    }

    protected abstract GameAction GetGameAction(EffectContext context, List<LaneView> laneTargets);
}
