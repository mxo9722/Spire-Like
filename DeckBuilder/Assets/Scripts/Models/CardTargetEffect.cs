using System.Collections.Generic;
using UnityEngine;

public abstract class CardTargetEffect : Effect
{
    public override GameAction GetGameAction(CombatantView caster, List<CombatantView> combatantTargets = null, List<LaneView> laneTargets = null, List<Card> cardTargets = null)
    {
        return GetGameAction(caster, cardTargets);
    }

    protected abstract GameAction GetGameAction(CombatantView caster, List<Card> cardTargets);
}
