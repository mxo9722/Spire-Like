using UnityEngine;

public class SavedDataQ : Quantity
{
    [SerializeField] private string _key;

    public override int GetAmount(EffectContext effectContext)
    {
        if (!effectContext.ContainsKey(_key))
            return 0;

        return effectContext.GetData<int>(_key);
    }

    public override int GetStaticAmount()
    {
        return 0;
    }
}
