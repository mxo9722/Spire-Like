using UnityEngine;

public class SetQ : Quantity
{
    [SerializeField] private int _set = 0;

    public override int GetStaticAmount()
    {
        return _set;
    }

    public override int GetAmount(EffectContext effectContext)
    {
        return _set;
    }
}
