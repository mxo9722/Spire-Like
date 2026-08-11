using SerializeReferenceEditor;
using System;
using UnityEngine;

public class DistanceFromCasterLaneFilter : LaneFilter
{
    [SerializeField] private NumberCompare _is;
    [SerializeReference, SR] private Quantity _value = new SetQ(1);

    protected override bool TargetIsValid(EffectContext context, LaneView target)
    {
        if (context.Caster == null)
            return false;

        LaneView casterLane = context.Caster.Lane;

        int dist = Math.Abs(casterLane.Index - target.Index);
        int value = _value.GetAmount(context);

        switch (_is)
        {
            case NumberCompare.LESS_THAN:
                return dist < value;
            case NumberCompare.EQUAL_TO:
                return dist == value;
            case NumberCompare.GREATER_THAN:
                return dist > value;
        }

        return false;
    }
}
