using SerializeReferenceEditor;
using System.Collections.Generic;
using UnityEngine;

public class LanesSoakedCondition : Condition
{

    [SerializeReference, SR] private LaneTargetMode _targetMode = new CurrentLTM();
    [SerializeField] private bool _checkForSoaked;

    protected override bool IsConditionMet(EffectContext context)
    {
        List<LaneView> lanes = _targetMode.GetTargets(context);

        if (lanes.Count == 0)
            return false;

        return lanes.TrueForAll(l => l.IsSoaked == _checkForSoaked);
    }

    public override bool IsConditionMeetable(EffectContext context, Card card)
    {
        List<LaneView> lanes = _targetMode.AllPossibleTargets(context, card);

        if (lanes.Count == 0)
            return false;

        return lanes.TrueForAll(l => l.IsSoaked == _checkForSoaked);
    }
}
