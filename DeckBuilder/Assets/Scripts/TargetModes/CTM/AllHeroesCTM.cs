using System.Collections.Generic;
using UnityEngine;

public class AllHeroesCTM : CombatantTargetMode
{
    public override NPCTargetTypes GetTargetIntent()
    {
        return NPCTargetTypes.FOCUS_HERO;
    }

    public override List<CombatantView> GetTargets(EffectContext targetModeContext)
    {
        return new(HeroSystem.Instance.HeroViews);
    }
}
