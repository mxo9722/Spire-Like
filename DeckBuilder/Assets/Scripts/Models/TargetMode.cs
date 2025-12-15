using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class TargetMode<T>
{
    public abstract List<T> GetTargets(EffectContext context);
    public virtual List<StatusEffectType> GetAllStatusEffects() { return null; }
    public virtual List<T> AllPossibleTargets(EffectContext context, Card card = null)
    {
        return GetTargets(context);
    }

    public virtual List<T> GetTargetsTrivial(EffectContext context) => GetTargets(context);

    public virtual bool IsRandom => false;

}
