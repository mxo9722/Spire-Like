using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MoveTowardLaneEffect : LaneTargetEffect
{

    protected override GameAction GetGameAction(EffectContext context, List<LaneView> laneViews)
    {
        if (laneViews == null || laneViews.Count == 0)
            return null;

        LaneView targetLaneView = laneViews[0];

        LaneView originalLaneView = BoardSystem.Instance.GetCurrentLaneView(context.Caster);

        if (targetLaneView == originalLaneView)
            return null;

        return new MoveUnitsGA(targetLaneView, context.Caster, context.Caster);
    }
}
