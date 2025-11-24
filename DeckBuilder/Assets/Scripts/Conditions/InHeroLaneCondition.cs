using System;
using UnityEngine;

[Serializable]
public class InHeroLaneCondition : Condition
{
    [field: SerializeField] public bool Invert = false;

    public override bool IsConditionMet(ConditionContext conditionalContext)
    {
        LaneView laneView = BoardSystem.Instance.GetCurrentLaneView(conditionalContext.Caster);

        if (Invert)
            return laneView.HeroView == null;
        return laneView.HeroView != null;
    }
}
