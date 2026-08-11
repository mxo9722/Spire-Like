using System.Collections.Generic;
using UnityEngine;

public class SavedDataCaTM : CardTargetMode
{
    [SerializeField] private string _dataKey = "TargetCards";
    [SerializeField] private SaveDataLevel _dataLevel = SaveDataLevel.CONTEXT;

    public override List<Card> GetTargets(EffectContext context)
    {
        IHoldData dataHolder = IHoldData.GetDataHolder(context, _dataLevel);

        List<Card> targets = dataHolder.GetData<List<Card>>(_dataKey);

        if (targets != null)
            return targets;

        return new();
    }
}
