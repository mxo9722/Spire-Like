using System.Collections.Generic;
using UnityEngine;

public class SavedDataCTM : CombatantTargetMode
{
    [SerializeField] private string _dataKey = "TargetUnits";
    [SerializeField] private SaveDataLevel _dataLevel = SaveDataLevel.CONTEXT;

    public override List<CombatantView> GetTargets(EffectContext context)
    {
        IHoldData dataHolder = IHoldData.GetDataHolder(context, _dataLevel);

        List<CombatantView> targets = dataHolder.GetData<List<CombatantView>>(_dataKey);

        if (targets != null)
            return targets;

        return new();
    }
}
