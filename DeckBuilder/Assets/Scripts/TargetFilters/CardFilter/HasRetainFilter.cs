using UnityEngine;

public class HasRetainFilter : CardFilter
{
    protected override bool TargetIsValid(EffectContext context, Card target)
    {
        return target.GetRetain();
    }
}
