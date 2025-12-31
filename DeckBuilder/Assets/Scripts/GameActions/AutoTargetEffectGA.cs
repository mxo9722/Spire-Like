using UnityEngine;

public class AutoTargetEffectGA : GameAction
{
    public EffectContext Context { get; private set; }
    public AutoTargetEffect Effect { get; private set; }

    public AutoTargetEffectGA(EffectContext context, AutoTargetEffect effect)
    {
        Context = context;
        Effect = effect;
    }

}
