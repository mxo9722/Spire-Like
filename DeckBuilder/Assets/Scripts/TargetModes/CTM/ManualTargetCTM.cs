using System.Collections.Generic;
using UnityEngine;

public class ManualTargetCTM : CombatantTargetMode
{
    public override List<CombatantView> GetTargets(EffectContext targetModeContext)
    {
        if(targetModeContext.TargetCombatant != null)
            return new() { targetModeContext.TargetCombatant };
        return new();
    }

    public override List<CombatantView> AllPossibleTargets(EffectContext context, Card card)
    {
        if (context.TargetCombatant != null)
            return new() { context.TargetCombatant };

        return new(card.AllValidCombatants(context));
    }
}
