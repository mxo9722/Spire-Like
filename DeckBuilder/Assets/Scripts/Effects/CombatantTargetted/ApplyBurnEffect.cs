using System.Collections.Generic;
using UnityEngine;

public class ApplyBurnEffect : CombatantTargetEffect
{
    protected override GameAction GetGameAction(EffectContext context, List<CombatantView> combatantTargets)
    {
        ApplyBurnGA applyBurnGA = new(combatantTargets);
        return applyBurnGA;
    }
}
