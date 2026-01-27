using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class KillUnitCTE : CombatantTargetEffect
{

    protected override GameAction GetGameAction(EffectContext context, List<CombatantView> combatantTargets)
    {
        KillNpcGA killNpcGA = new(combatantTargets.Cast<NPCView>().ToList());

        return killNpcGA;
    }
}
