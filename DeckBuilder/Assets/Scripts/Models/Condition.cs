using System;
using UnityEngine;

[Serializable]
public abstract class Condition
{
    public abstract bool IsConditionMet(ConditionContext conditionalContext);
}
