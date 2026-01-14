using System.Collections.Generic;
using UnityEngine;

public class HeroCTM : CombatantTargetMode
{
    public override NPCTargetTypes GetTargetIntent()
    {
        return NPCTargetTypes.FOCUS_TARGET;
    }

    public override List<CombatantView> GetTargets(EffectContext targetModeContext)
    {
        return new() { HeroSystem.Instance.HeroView };
    }
}
