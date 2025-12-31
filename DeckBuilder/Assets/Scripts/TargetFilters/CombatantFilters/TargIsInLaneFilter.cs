using SerializeReferenceEditor;
using UnityEngine;

public class TargIsInLaneFilter : CombatantFilter
{

    [SerializeReference, SR] private LaneTargetMode _lane = new CurrentLTM();

    protected override bool TargetIsValid(EffectContext context, CombatantView target)
    {
        System.Collections.Generic.List<LaneView> lanes = _lane.GetTargets(context);

        LaneView targetLane = BoardSystem.Instance.GetCurrentLaneView(target);

        return lanes.Contains(targetLane);
    }
}
