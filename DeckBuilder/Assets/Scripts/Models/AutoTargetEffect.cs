using System.Collections.Generic;

[System.Serializable]
public abstract class AutoTargetEffect
{
    public abstract Effect Effect { get; }

    public abstract GameAction GetGameAction(TargetModeContext targetModeContext);
    public IDynamicEffectText GetDynamicTextEffect()
    {
        if (Effect is IDynamicEffectText dynamicEffectText)
            return dynamicEffectText;
        return null;
    }

    public abstract string GetDynamicText(TargetModeContext targetModeContext);

    public abstract List<StatusEffectType> GetAllStatusEffects();
}
