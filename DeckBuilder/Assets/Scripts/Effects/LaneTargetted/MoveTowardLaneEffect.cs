using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MoveTowardLaneEffect : LaneTargetEffect
{

    protected override GameAction GetGameAction(CombatantView caster, List<LaneView> laneViews)
    {
        if (laneViews == null || laneViews.Count == 0)
            return null;

        LaneView targetLaneView = laneViews[0];

        LaneView originalLaneView = BoardSystem.Instance.GetCurrentLaneView(caster);

        if (targetLaneView == originalLaneView)
            return null;

        if (caster is HeroView heroView)
        {
            return new MoveHeroGA(targetLaneView, heroView);
        }
        else if (caster is EnemyView enemyView)
        {
            return new MoveEnemyGA(targetLaneView, enemyView);
        }

        return null;
    }
}
