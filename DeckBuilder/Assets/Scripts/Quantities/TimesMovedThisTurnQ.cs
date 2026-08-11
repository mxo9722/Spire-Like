using SerializeReferenceEditor;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TimesMovedThisTurnQ : Quantity
{
    [SerializeReference, SR] private CombatantTargetMode combatantTargetMode = new CasterCTM(); 

    public override int GetAmount(EffectContext effectContext)
    {
        List<CombatantView> targets = combatantTargetMode.GetTargets(effectContext);

        if (targets.Count == 0)
        {
            return 0;
        }

        return targets.Sum(t => t.GetCurrentRoundMovement());
    }

    public override int GetStaticAmount()
    {
        return 0;
    }
}
