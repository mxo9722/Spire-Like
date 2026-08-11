using UnityEngine;

public class HandSizeQ : Quantity
{
    public override int GetAmount(EffectContext effectContext)
    {
        return CardSystem.Instance.GetCardCount();
    }

    public override int GetStaticAmount()
    {
        return 0;
    }
}
