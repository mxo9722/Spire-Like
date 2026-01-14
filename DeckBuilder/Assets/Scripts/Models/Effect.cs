using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public abstract class Effect
{
    public abstract GameAction GetGameAction(EffectContext context, List<CombatantView> combatantTargets = null, List<LaneView> laneTargets = null, List<Card> cardTargets = null);

    public virtual List<StatusEffectType> GetAllStatusEffects() { return null; }

    public virtual IDynamicEffectText[] GetDynamicTextEffects() 
    {
        if (this is IDynamicEffectText dynamicEffectText)
            return new[] { dynamicEffectText };
        return new IDynamicEffectText[0];
    }
}
