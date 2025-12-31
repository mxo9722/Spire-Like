using System.Collections.Generic;
using UnityEngine;

public class SavedDataCTM : CombatantTargetMode
{
    [SerializeField] private string _dataKey = "TargetUnits";

    public override List<CombatantView> GetTargets(EffectContext context)
    {
        List<CombatantView> targets = context.GetData<List<CombatantView>>(_dataKey);

        if (targets != null)
            return targets;

        return new();
    }
}
