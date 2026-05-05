using SerializeReferenceEditor;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitsInLaneFilter : LaneFilter
{
    [SerializeReference, SR] private CombatantTargetMode _targetMode;

    protected override bool TargetIsValid(EffectContext context, LaneView target)
    {
        List<CombatantView> units = _targetMode.GetTargets(context);
        return units.Any(u => u.Lane == target);
    }
}
