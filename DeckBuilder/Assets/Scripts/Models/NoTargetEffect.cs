using System.Collections.Generic;
using UnityEngine;

public abstract class NoTargetEffect : Effect
{
    public override GameAction GetGameAction(EffectContext context, List<CombatantView> combatantTargets = null, List<LaneView> laneTargets = null, List<Card> cards = null)
    {
        return GetGameAction(context);
    }

    protected abstract GameAction GetGameAction(EffectContext context);
}
