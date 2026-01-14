using UnityEngine;

public class HandSizeQ : Quantity
{
    public override int GetAmount(EffectContext effectContext)
    {
        return GetStaticAmount();
    }

    public override int GetStaticAmount()
    {
        return CardSystem.Instance.GetCardCount();
    }
}
