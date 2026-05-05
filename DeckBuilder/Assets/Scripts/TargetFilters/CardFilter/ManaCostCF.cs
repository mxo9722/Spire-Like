using UnityEngine;

public class ManaCostCF : CardFilter
{
    [SerializeField] private NumberCompare _manaIs;
    [SerializeField] private int _value;

    protected override bool TargetIsValid(EffectContext context, Card target)
    {
        int manaValue = target.GetDynamicManaValue(context);

        switch (_manaIs)
        {
            case NumberCompare.LESS_THAN:
                return manaValue < _value;
            case NumberCompare.EQUAL_TO:
                return manaValue == _value;
            case NumberCompare.GREATER_THAN:
                return manaValue > _value;
        }

        return false;
    }
}

