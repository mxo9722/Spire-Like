using SerializeReferenceEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HeroTargettedEventAction : EventAction
{
    [SerializeReference, SR] private HeroTargetMode TargetMode;

    public override IEnumerator Invoke(EffectContext context)
    {
        var heroes = TargetMode.GetTargets(context);
        yield return Invoke(context, heroes);
    }

    protected abstract IEnumerator Invoke(EffectContext context, List<Hero> targets);
}
