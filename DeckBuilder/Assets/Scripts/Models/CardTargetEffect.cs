using System.Collections.Generic;
using UnityEngine;

public abstract class CardTargetEffect : Effect
{
    public override GameAction GetGameAction(EffectContext context, List<CombatantView> combatantTargets = null, List<LaneView> laneTargets = null, List<Card> cardTargets = null)
    {
        return GetGameAction(context, cardTargets);
    }

    protected abstract GameAction GetGameAction(EffectContext context, List<Card> cardTargets);
}
