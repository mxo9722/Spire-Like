using UnityEngine;

public class LaneCountQ : Quantity
{
    public override int GetAmount(EffectContext effectContext)
    {
        return BoardSystem.Instance.GetAllLanes().Count;
    }

    public override int GetStaticAmount()
    {
        return BoardSystem.Instance.GetAllLanes().Count;
    }
}
