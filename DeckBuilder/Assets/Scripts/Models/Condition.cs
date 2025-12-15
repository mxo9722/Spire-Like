using System;
using UnityEngine;

[Serializable]
public abstract class Condition
{
    [SerializeField] private bool _invert = false;

    public bool TestCondition(EffectContext context)
    {
        if (_invert)
            return !IsConditionMet(context);
        return IsConditionMet(context);
    }

    protected abstract bool IsConditionMet(EffectContext context);

    public virtual bool IsConditionMeetable(EffectContext context, Card card)
    {
        return IsConditionMet(context);
    }
}
