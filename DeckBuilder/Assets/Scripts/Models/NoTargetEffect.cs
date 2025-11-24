using System.Collections.Generic;
using UnityEngine;

public abstract class NoTargetEffect : Effect
{
    public override GameAction GetGameAction(CombatantView caster, List<CombatantView> combatantTargets = null, List<LaneView> laneTargets = null)
    {
        return GetGameAction(caster);
    }

    protected abstract GameAction GetGameAction(CombatantView caster);
}
