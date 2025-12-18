using UnityEngine;

public abstract class Quantity
{
    public abstract int GetStaticAmount();
    public abstract int GetAmount(EffectContext effectContext);
}
