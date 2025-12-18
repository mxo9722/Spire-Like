using System.Collections.Generic;
using UnityEngine;

public abstract class CombatantTargetEffect : Effect
{
    public override GameAction GetGameAction(EffectContext context, List<CombatantView> combatantTargets = null, List<LaneView> laneTargets = null, List<Card> cardTargets = null)
    {
        return GetGameAction(context, combatantTargets);
    }

    protected abstract GameAction GetGameAction(EffectContext context, List<CombatantView> combatantTargets);
}
