using UnityEngine;

public class RenownQ : Quantity
{
    public override int GetAmount(EffectContext effectContext)
    {
        return GetStaticAmount();
    }

    public override int GetStaticAmount()
    {
        return RenownSystem.Instance.Renown;
    }
}
