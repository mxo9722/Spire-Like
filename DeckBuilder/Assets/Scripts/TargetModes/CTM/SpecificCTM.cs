using System.Collections.Generic;
using UnityEngine;

public class SpecificCTM : CombatantTargetMode
{

    private CombatantView _target;

    public SpecificCTM(CombatantView target)
    {
        _target = target;
    }

    public override List<CombatantView> GetTargets(EffectContext context)
    {
        return new() { _target };
    }
}
