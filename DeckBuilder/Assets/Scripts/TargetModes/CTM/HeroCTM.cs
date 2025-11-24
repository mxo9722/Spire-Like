using System.Collections.Generic;
using UnityEngine;

public class HeroCTM : CombatantTargetMode
{
    public override List<CombatantView> GetTargets(TargetModeContext targetModeContext)
    {
        return new() { HeroSystem.Instance.HeroView };
    }
}
