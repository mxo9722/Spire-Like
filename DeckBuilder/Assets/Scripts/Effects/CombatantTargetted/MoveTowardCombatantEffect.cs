using System.Collections.Generic;
using UnityEngine;

public class MoveTowardCombatantEffect : CombatantTargetEffect
{

    protected override GameAction GetGameAction(EffectContext context, List<CombatantView> combatantTargets)
    {
        if ((combatantTargets == null || combatantTargets.Count == 0))
            return null;

        CombatantView targetView = combatantTargets[0];
        LaneView targetLaneView = BoardSystem.Instance.GetCurrentLaneView(targetView);

        return new MoveUnitsGA(targetLaneView, context.Caster, context.Caster);
    }
}
