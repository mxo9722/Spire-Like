using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class LaneTargetEffect : Effect
{
    public override GameAction GetGameAction(CombatantView caster, List<CombatantView> combatantTargets = null, List<LaneView> laneTargets = null, List<Card> cardTargets = null)
    {
        return GetGameAction(caster, laneTargets);
    }

    protected abstract GameAction GetGameAction(CombatantView caster, List<LaneView> laneTargets);
}
