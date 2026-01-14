using System.Collections.Generic;
using UnityEngine;

public class MultipleEffectsGA : GameAction
{
    public EffectContext Context { get; private set; }
    public List<AutoTargetEffect> Effects { get; private set; }

    public MultipleEffectsGA(EffectContext context, List<AutoTargetEffect> effects)
    {
        Context = context;
        Effects = effects;
    }
}
