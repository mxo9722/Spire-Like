using System.Collections.Generic;
using UnityEngine;

public class RelativeDirectionLTM : LaneTargetMode
{

    [SerializeField] private MovementDirection _direction;

    public override List<LaneView> GetTargets(EffectContext context)
    {
        LaneView curLane = BoardSystem.Instance.GetCurrentLaneView(context.Caster);

        LaneView lane = BoardSystem.Instance.GetLaneFromDirection(curLane, _direction);

        return new() { lane };
    }
}
