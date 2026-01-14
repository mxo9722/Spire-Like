using UnityEngine;

public class RenownMinimumEC : EventCondition
{
    [SerializeField, Min(1)] private int _renownMinimumNeeded = 100;

    public override bool IsMet()
    {
        return RenownSystem.Instance.Renown >= _renownMinimumNeeded;
    }
}
