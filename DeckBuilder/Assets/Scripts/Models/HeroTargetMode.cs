using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class HeroTargetMode
{
    public abstract List<Hero> GetTargets(EffectContext context);
}
