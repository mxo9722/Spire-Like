using System.Collections.Generic;
using UnityEngine;

public class EnemiesInCurrentLaneTM : CombatantTargetMode
{
    public override List<CombatantView> GetTargets(EffectContext targetModeContext)
    {
        LaneView laneView = null;

        if (targetModeContext.Caster is EnemyView enemyView)
            laneView = BoardSystem.Instance.GetCurrentLaneView(enemyView);
        else if (targetModeContext.Caster is HeroView heroView)
            laneView = BoardSystem.Instance.GetCurrentLaneView(heroView);

        if(laneView == null)
            return new();

        return new(laneView.EnemyViews);
    }
}
