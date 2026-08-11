using SerializeReferenceEditor;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CombatantsWithFilterCTM : CombatantTargetMode
{

    [SerializeReference, SR] private List<CombatantFilter> _filters;

    public override List<CombatantView> GetTargets(EffectContext context)
    {
        List<CombatantView> allCombatants = BoardSystem.Instance.GetAllCombatants();

        allCombatants.RemoveAll(c => !_filters.TrueForAll(f => f.TestTarget(context, c)));

        return allCombatants;
    }
}
