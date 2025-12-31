using SerializeReferenceEditor;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombatantCountQ : Quantity
{
    [SerializeReference, SR] private CombatantTargetMode _targetMode;

    [SerializeReference, SR] private List<CombatantFilter> _filters;

    public override int GetAmount(EffectContext effectContext)
    {
        return _targetMode.GetTargets(effectContext).Where(c => _filters.TrueForAll(f => f.TestTarget(effectContext, c))).Count();
    }

    public override int GetStaticAmount()
    {
        EffectContext effectContext = new();
        return _targetMode.GetTargets(effectContext).Where(c => _filters.TrueForAll(f => f.TestTarget(effectContext, c))).Count();
    }
}
