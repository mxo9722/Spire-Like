using System.Collections.Generic;
using UnityEngine;

public class HeroInCurrentLaneTM : TargetMode
{
    public override List<CombatantView> GetTargets(TargetModeContext targetModeContext)
    {
        LaneView laneView = null;

        if (targetModeContext.Caster is EnemyView enemyView)
            laneView = BoardSystem.Instance.GetCurrentLaneView(enemyView);
        else if (targetModeContext.Caster is HeroView heroView)
            laneView = BoardSystem.Instance.GetCurrentLaneView(heroView);

        if (laneView == null || laneView.HeroView == null)
            return new();

        return new() { laneView.HeroView};
    }
}
