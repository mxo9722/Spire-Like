using System.Collections.Generic;
using UnityEngine;

public class SetSoakEffect : LaneTargetEffect
{

    [SerializeField] private bool _setSoaked;

    protected override GameAction GetGameAction(EffectContext context, List<LaneView> laneTargets)
    {
        SetSoakGA setSoakGA = new(laneTargets, _setSoaked);

        return setSoakGA;
    }
}
