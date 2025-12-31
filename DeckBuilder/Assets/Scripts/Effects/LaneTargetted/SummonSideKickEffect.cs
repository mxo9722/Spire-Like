using System.Collections.Generic;
using UnityEngine;

public class SummonSideKickEffect : LaneTargetEffect
{

    [SerializeField] private NPCData _data;

    protected override GameAction GetGameAction(EffectContext context, List<LaneView> laneTargets)
    {
        SummonSideKickGA summonSideKickGA = new(laneTargets[0], _data);
        return summonSideKickGA;
    }

}
