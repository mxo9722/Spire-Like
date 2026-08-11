using System.Collections.Generic;
using UnityEngine;

public class AllHTM : HeroTargetMode
{
    public override List<Hero> GetTargets(EffectContext context)
    {
        RunData runData = RunSystem.Instance.RunData;

        return new() { runData.Hero1, runData.Hero2 };
    }
}
