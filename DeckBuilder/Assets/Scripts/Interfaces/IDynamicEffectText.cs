using System.Collections.Generic;
using UnityEngine;

public interface IDynamicEffectText
{
    public abstract string GetStaticText();
    public abstract string GetDynamicText(EffectContext context, List<CombatantView> targetCombatants = null, List<LaneView> targetLanes = null);
}
