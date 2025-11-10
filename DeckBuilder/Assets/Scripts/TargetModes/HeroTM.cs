using System.Collections.Generic;
using UnityEngine;

public class HeroTM : TargetMode
{
    public override List<CombatantView> GetTargets(TargetModeContext targetModeContext)
    {
        return new() { HeroSystem.Instance.HeroView };
    }
}
