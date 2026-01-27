using System.Collections.Generic;
using UnityEngine;

public abstract class SavedListSizeQ<T> : Quantity
{
    [SerializeField] private SaveDataLevel _dataLevel;
    [SerializeField] private string _saveKey;

    public override int GetAmount(EffectContext context)
    {
        IHoldData dataHolder = IHoldData.GetDataHolder(context, _dataLevel);

        List<T> list = dataHolder.GetData<List<T>>(_saveKey);

        if (list == null)
            return 0;

        return list.Count;
    }

    public override int GetStaticAmount()
    {
        IHoldData dataHolder = IHoldData.GetDataHolder(new(), _dataLevel);

        List<T> list = dataHolder.GetData<List<T>>(_saveKey);

        if (list == null)
            return 0;

        return list.Count;
    }
}

public class SavedCombatantListSizeQ : SavedListSizeQ<CombatantView> { }
public class SavedLaneListSizeQ : SavedListSizeQ<LaneView> { }