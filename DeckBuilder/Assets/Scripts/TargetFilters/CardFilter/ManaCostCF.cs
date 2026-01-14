using UnityEngine;

public class ManaCostCF : CardFilter
{
    [SerializeField] private NumberCompare _manaIs;
    [SerializeField] private int _value;

    protected override bool TargetIsValid(EffectContext context, Card target)
    {
        switch (_manaIs)
        {
            case NumberCompare.LESS_THAN:
                return target.Mana < _value;
            case NumberCompare.EQUAL_TO:
                return target.Mana == _value;
            case NumberCompare.GREATER_THAN:
                return target.Mana > _value;
        }

        return false;
    }
}

