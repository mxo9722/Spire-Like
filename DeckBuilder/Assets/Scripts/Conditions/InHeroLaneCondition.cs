using System;
using UnityEngine;

[Serializable]
public class InHeroLaneCondition : Condition
{
    protected override bool IsConditionMet(EffectContext context)
    {
        LaneView laneView = context.Caster.Lane;

        return laneView?.HeroView != null;
    }
}
