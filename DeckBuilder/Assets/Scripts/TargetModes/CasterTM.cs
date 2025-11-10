using System.Collections.Generic;
using UnityEngine;

public class CasterTM : TargetMode
{
    public override List<CombatantView> GetTargets(TargetModeContext targetModeContext)
    {
        return new() { targetModeContext.Caster };
    }
}
