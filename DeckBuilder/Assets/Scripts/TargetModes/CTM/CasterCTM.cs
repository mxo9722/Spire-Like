using System.Collections.Generic;
using UnityEngine;

public class CasterCTM : CombatantTargetMode
{
    public override List<CombatantView> GetTargets(EffectContext targetModeContext)
    {
        return new() { targetModeContext.Caster };
    }

    public override EnemyTargetTypes GetTargetIntent()
    {
        return EnemyTargetTypes.SELF;
    }
}
