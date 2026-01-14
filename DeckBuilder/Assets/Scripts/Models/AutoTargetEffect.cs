using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public abstract class AutoTargetEffect
{
    public abstract Effect Effect { get; }

    public abstract GameAction GetGameAction(EffectContext targetModeContext);
    public virtual IDynamicEffectText[] GetDynamicTextEffects()
    {
        return Effect.GetDynamicTextEffects();
    }

    public virtual bool HasDynamicTextEffects()
    {
        return GetDynamicTextEffects().Length > 0;
    }

    public virtual string ApplyDynamicTextEffect(string description, int startIndex, EffectContext context, Card card)
    {
        IDynamicEffectText[] dtes = GetDynamicTextEffects();
        
        foreach(IDynamicEffectText dte in dtes)
        {
            string value = dte.GetDynamicText(context);
            description = description.Replace("{v"+(startIndex++)+"}", value);
        }

        return description;
    }

    public abstract bool RequiresUserInput();
    public abstract IEnumerator WaitForUserInput();

    public abstract List<StatusEffectType> GetAllStatusEffects();
}
