using UnityEngine;

public class SavedDataTrueCondition : Condition
{
    [SerializeField] private string _key;
    [SerializeField] private SaveDataLevel _dataLevel = SaveDataLevel.CONTEXT;

    protected override bool IsConditionMet(EffectContext context)
    {
        IHoldData dataHolder = IHoldData.GetDataHolder(context, _dataLevel);

        if (!dataHolder.ContainsKey(_key)) return false;
        return dataHolder.GetData<bool>(_key);
    }
}
