using SerializeReferenceEditor;
using UnityEngine;

public class QuantityCompareCondition : Condition
{

    [SerializeReference, SR] private Quantity _a;
    [SerializeField] private NumberCompare _is;
    [SerializeField] private bool _orEqualTo = false;
    [SerializeReference, SR] private Quantity _b;

    protected override bool IsConditionMet(EffectContext context)
    {
        int a = _a.GetAmount(context);
        int b = _b.GetAmount(context);

        if (_orEqualTo && a == b)
            return true;

        switch (_is)
        {
            case NumberCompare.LESS_THAN:
                return a < b;
            case NumberCompare.EQUAL_TO:
                return a == b;
            case NumberCompare.GREATER_THAN:
                return a > b;
        }

        return false;
    }
}
