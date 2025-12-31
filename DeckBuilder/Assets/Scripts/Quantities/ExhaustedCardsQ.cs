using UnityEngine;

public class ExhaustedCardsQ : Quantity
{
    public override int GetAmount(EffectContext effectContext)
    {
        return CardSystem.Instance.GetExhaustPile().Count;
    }

    public override int GetStaticAmount()
    {
        return 0;
    }
}
