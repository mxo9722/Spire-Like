using UnityEngine;

public class RoundNumberQ : Quantity
{
    public override int GetAmount(EffectContext effectContext)
    {
        return GetStaticAmount();
    }

    public override int GetStaticAmount()
    {
        return CombatTrackerSystem.Instance.Round;
    }
}
