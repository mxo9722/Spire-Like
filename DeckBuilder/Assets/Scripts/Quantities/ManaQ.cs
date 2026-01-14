using UnityEngine;

public class ManaQ : Quantity
{
    public override int GetAmount(EffectContext effectContext)
    {
        return ManaSystem.Instance.CurrentMana;
    }

    public override int GetStaticAmount()
    {
        return 0;
    }
}
