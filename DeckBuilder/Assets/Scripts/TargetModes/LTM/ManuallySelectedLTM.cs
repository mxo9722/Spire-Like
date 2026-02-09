using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ManuallySelectedLTM : LaneTargetMode
{
    public override List<LaneView> GetTargets(EffectContext targetModeContext)
    {
        if(targetModeContext.TargetLane != null)
            return new() { targetModeContext.TargetLane };
        return new();
    }
}
