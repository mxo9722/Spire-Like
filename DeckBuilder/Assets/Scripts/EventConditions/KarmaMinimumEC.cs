using UnityEngine;

public class KarmaMinimumEC : EventCondition
{
    [SerializeField, Min(1)] private int _creditMinimumNeeded = 100;

    public override bool IsMet()
    {
        return KarmaSystem.Instance.Karma >= _creditMinimumNeeded;
    }
}
