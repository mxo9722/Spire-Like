using UnityEngine;

public class SavedDataQ : Quantity
{
    [SerializeField] private SaveDataLevel _level;
    [SerializeField] private string _key;

    public override int GetAmount(EffectContext effectContext)
    {
        IHoldData dataHolder = IHoldData.GetDataHolder(effectContext, _level);

        if (dataHolder == null || !dataHolder.ContainsKey(_key))
            return 0;

        return dataHolder.GetData<int>(_key);
    }

    public override int GetStaticAmount()
    {
        return 0;
    }
}
