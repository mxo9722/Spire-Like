using UnityEngine;

public class AlwaysTrueCondition : Condition
{
    protected override bool IsConditionMet(EffectContext context)
    {
        return true;
    }
}
