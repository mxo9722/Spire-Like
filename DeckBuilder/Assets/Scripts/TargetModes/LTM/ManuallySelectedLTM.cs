using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ManuallySelectedLTM : LaneTargetMode
{
    public override List<LaneView> GetTargets(EffectContext targetModeContext)
    {
        return new() { targetModeContext.TargetLane };
    }
}
