using SerializeReferenceEditor;
using System.Collections.Generic;
using UnityEngine;

public class AllCTM : CombatantTargetMode
{
    [SerializeReference, SR] private List<CombatantFilter> _filters;

    public override List<CombatantView> GetTargets(EffectContext context)
    {
        return new(BoardSystem.Instance.GetAllCombatants().ApplyFilters(_filters, context));
    }
}
