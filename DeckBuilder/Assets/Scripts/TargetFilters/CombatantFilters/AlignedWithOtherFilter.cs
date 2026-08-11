using SerializeReferenceEditor;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AlignedWithOtherFilter : CombatantFilter
{
    [SerializeReference, SR] private CombatantTargetMode _targetMode;

    protected override bool TargetIsValid(EffectContext context, CombatantView target)
    {
        List<CombatantView> targets = _targetMode.GetTargets(context);

        if (context.Caster != null)
            targets.Remove(context.Caster);

        return targets.Any(t => t.Lane == target.Lane);
    }
}
