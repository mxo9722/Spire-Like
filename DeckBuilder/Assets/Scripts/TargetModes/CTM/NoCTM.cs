using System.Collections.Generic;
using UnityEngine;

public class NoCTM : CombatantTargetMode
{
    public override List<CombatantView> GetTargets(TargetModeContext targetModeContext)
    {
        return null;
    }
}
