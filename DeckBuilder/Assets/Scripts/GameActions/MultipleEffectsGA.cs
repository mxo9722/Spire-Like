using System.Collections.Generic;
using UnityEngine;

public class MultipleEffectsGA : SimulatedGameAction
{
    public EffectContext Context { get; private set; }
    public List<AutoTargetEffect> Effects { get; private set; }

    public MultipleEffectsGA(EffectContext context, List<AutoTargetEffect> effects)
    {
        Context = context;
        Effects = effects;
    }

    public override void SimulatedPerform(EffectContext context)
    {
        foreach(AutoTargetEffect effect in Effects)
        {
            effect.SimulatedPerform(context);
        }
    }
}
