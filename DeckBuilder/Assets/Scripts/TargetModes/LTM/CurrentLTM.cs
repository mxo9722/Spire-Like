using System.Collections.Generic;
using UnityEngine;

public class CurrentLTM : LaneTargetMode
{
    public override List<LaneView> GetTargets(EffectContext context)
    {
        if(context.Caster != null)
            return new() { BoardSystem.Instance.GetCurrentLaneView(context.Caster) };
        return new();
    }
}
