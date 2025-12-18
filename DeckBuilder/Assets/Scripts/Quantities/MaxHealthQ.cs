using UnityEngine;

public class MaxHealthQ : Quantity
{
    public override int GetAmount(EffectContext effectContext)
    {
        return RunSystem.Instance.MaxHealth;
    }

    public override int GetStaticAmount()
    {
        return RunSystem.Instance.MaxHealth;
    }
}
