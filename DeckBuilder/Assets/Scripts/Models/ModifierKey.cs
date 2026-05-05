using UnityEngine;

public abstract class ModifierKey
{
    public EffectContext Context { get; private set; }
    public ModifierKey(EffectContext context)
    {
        Context = context;
    }
}
