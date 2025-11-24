using System.Collections.Generic;
using UnityEngine;

public class CasterCTM : CombatantTargetMode
{
    public override List<CombatantView> GetTargets(TargetModeContext targetModeContext)
    {
        return new() { targetModeContext.Caster };
    }

    public override EnemyTargetTypes GetTargetIntent()
    {
        return EnemyTargetTypes.SELF;
    }
}
