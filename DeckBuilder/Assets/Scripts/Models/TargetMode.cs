using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class TargetMode<T>
{
    public abstract List<T> GetTargets(TargetModeContext targetModeContext);
    public virtual List<StatusEffectType> GetAllStatusEffects() { return null; }

}
