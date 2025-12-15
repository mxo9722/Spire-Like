using UnityEngine;

public class CreditMinimumEC : EventCondition
{
    [SerializeField, Min(1)] private int _creditMinimumNeeded = 100;

    public override bool IsMet()
    {
        return CreditSystem.Instance.Credits >= _creditMinimumNeeded;
    }
}
