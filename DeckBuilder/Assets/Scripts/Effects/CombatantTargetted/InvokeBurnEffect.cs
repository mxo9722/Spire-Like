using System.Collections.Generic;
using UnityEngine;

public class InvokeBurnEffect : CombatantTargetEffect
{
    protected override GameAction GetGameAction(EffectContext context, List<CombatantView> combatantTargets)
    {
        InvokeBurnGA invokeBurnGA = new(combatantTargets);
        return invokeBurnGA;
    }
}
