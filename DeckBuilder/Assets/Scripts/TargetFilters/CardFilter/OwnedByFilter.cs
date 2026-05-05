using SerializeReferenceEditor;
using System.Collections.Generic;
using UnityEngine;

public class OwnedByFilter : CardFilter
{
    [SerializeReference, SR] private CombatantTargetMode _targetUnits;
    [SerializeField] private bool _acceptNoTarget = false;

    protected override bool TargetIsValid(EffectContext context, Card target)
    {
        List<CombatantView> units = _targetUnits.GetTargets(context);
        HeroView owner = target.GetOwnerView(context);

        if (owner == null)
            return _acceptNoTarget;

        if (units.Contains(owner))
            return true;

        return false;
    }
}
