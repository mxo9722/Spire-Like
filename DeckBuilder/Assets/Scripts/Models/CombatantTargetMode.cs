
using System;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public abstract class CombatantTargetMode : TargetMode<CombatantView>
{
    public virtual bool LaneRelevant(EffectContext context, LaneView lane)
    {
        List<CombatantView> targets = GetTargets(context);

        return targets.Any(t => lane.Contains(t));
    }

    public virtual bool HostileTargetting(EffectContext context)
    {
        return false;
    }
}
