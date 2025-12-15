using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public abstract class AutoTargetEffect
{
    public abstract Effect Effect { get; }

    public abstract GameAction GetGameAction(EffectContext targetModeContext);
    public IDynamicEffectText GetDynamicTextEffect()
    {
        if (Effect is IDynamicEffectText dynamicEffectText)
            return dynamicEffectText;
        return null;
    }

    public abstract bool RequiresUserInput();
    public abstract IEnumerator WaitForUserInput();

    public abstract string GetDynamicText(EffectContext targetModeContext);

    public abstract List<StatusEffectType> GetAllStatusEffects();
}
