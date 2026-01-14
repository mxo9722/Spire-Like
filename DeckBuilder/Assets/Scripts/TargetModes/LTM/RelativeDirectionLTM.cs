using System.Collections.Generic;
using UnityEngine;

public class RelativeDirectionLTM : LaneTargetMode
{

    [SerializeField] private MovementDirection _direction;
    [SerializeField] private int _moveCount = 1;
    [SerializeField] private bool _loopAround;

    public override List<LaneView> GetTargets(EffectContext context)
    {
        LaneView curLane = BoardSystem.Instance.GetCurrentLaneView(context.Caster);

        LaneView lane = BoardSystem.Instance.GetLaneFromDirection(curLane, _direction, _moveCount, _loopAround);

        return new() { lane };
    }
}
