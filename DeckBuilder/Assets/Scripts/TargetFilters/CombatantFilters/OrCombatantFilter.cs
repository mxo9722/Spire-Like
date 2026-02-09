using SerializeReferenceEditor;
using System.Collections.Generic;
using UnityEngine;

public class OrCombatantFilter : CombatantFilter
{

    [SerializeField] private List<FilterCollection> _filterCases = new() { new(), new() };

    protected override bool TargetIsValid(EffectContext context, CombatantView target)
    {
        foreach(FilterCollection filterCase in _filterCases)
        if (filterCase.filters.Count > 0 && filterCase.filters.TrueForAll(c => c.TestTarget(context, target)))
                return true;

        return false;
    }

    [System.Serializable]
    private class FilterCollection
    {
        [SerializeReference, SR] public List<CombatantFilter> filters;
    }
}