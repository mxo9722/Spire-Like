using System;
using UnityEngine;

[Serializable]
public class InHeroLaneCondition : Condition
{
    protected override bool IsConditionMet(EffectContext context)
    {
        LaneView laneView = BoardSystem.Instance.GetCurrentLaneView(context.Caster);

        return laneView?.HeroView != null;
    }
}
