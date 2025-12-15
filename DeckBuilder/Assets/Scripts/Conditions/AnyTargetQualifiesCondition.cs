using SerializeReferenceEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class AnyTargetQualifiesCondition : Condition
{
    [SerializeReference, SR] private CombatantTargetMode _targetMode;
    [SerializeReference, SR] private List<CombatantFilter> _filters;

    protected override bool IsConditionMet(EffectContext context)
    {
        List<CombatantView> targets = _targetMode.GetTargets(context);

        foreach (CombatantView target in targets)
        {
            bool qualifies = true;

            foreach (CombatantFilter filter in _filters)
            {
                if (!filter.TestTarget(context, target))
                {
                    qualifies = false;
                    break;
                }
            }

            if (qualifies)
                return true;
        }

        return false;
    }

    public override bool IsConditionMeetable(EffectContext context, Card card)
    {
        List<CombatantView> targets = _targetMode.AllPossibleTargets(context,card);

        return targets.Any(t => _filters.TrueForAll(f => f.TestTarget(context, t)));
    }
}
