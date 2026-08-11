using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public abstract class AutoTargetEffect
{
    public abstract Effect[] Effects { get; }

    public abstract GameAction GetGameAction(EffectContext targetModeContext);
    public virtual IDynamicEffectText[] GetDynamicTextEffects()
    {
        List<IDynamicEffectText> dets = new();

        foreach(Effect effect in Effects)
        {
            dets.AddRange(effect.GetDynamicTextEffects());
        }

        return dets.ToArray();
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

    public virtual NPCTargetTypes GetTargetIntent()
    {
        return NPCTargetTypes.NONE;
    }

    public virtual AutoTargetEffect[] GetNestedEffects()
    {
        return new AutoTargetEffect[0];
    }

    public List<StatusEffect> GetAllStatusEffects()
    {
        List<StatusEffect> effects = new();

        effects.AddRange(Effects.SelectMany(e => e.GetAllStatusEffects()));

        return new(effects.Distinct());
    }

    public abstract bool RequiresUserInput();
    public abstract IEnumerator WaitForUserInput(EffectContext context);

    public abstract void SimulatedPerform(EffectContext context);
}
