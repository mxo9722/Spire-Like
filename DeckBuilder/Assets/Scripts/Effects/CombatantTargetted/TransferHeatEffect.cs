using System.Collections.Generic;
using UnityEngine;

public class TransferHeatEffect : CombatantTargetEffect
{
    protected override GameAction GetGameAction(EffectContext context, List<CombatantView> combatantTargets)
    {
        return new TransferHeatGA(combatantTargets, context);
    }
}
