using System.Collections.Generic;
using UnityEngine;

public interface IDynamicEffectText
{
    public abstract string GetStaticText();
    public abstract string GetDynamicText(CombatantView caster, List<CombatantView> targetCombatants = null, List<LaneView> targetLanes = null);
}
