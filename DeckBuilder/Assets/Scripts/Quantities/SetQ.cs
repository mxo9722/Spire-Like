using UnityEngine;

public class SetQ : Quantity
{
    [SerializeField] private int _set = 0;

    public SetQ() { }

    public SetQ(int set)
    {
        _set = set;
    }

    public override int GetStaticAmount()
    {
        return _set;
    }

    public override int GetAmount(EffectContext effectContext)
    {
        return _set;
    }
}
