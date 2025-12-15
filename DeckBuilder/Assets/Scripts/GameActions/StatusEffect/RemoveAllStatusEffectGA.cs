using System.Collections.Generic;
using UnityEngine;

public class RemoveAllStatusEffectGA : GameAction
{
    public StatusEffectType StatusEffectType { get; private set; }
    public List<CombatantView> Targets { get; private set; }

    public RemoveAllStatusEffectGA(StatusEffectType type, List<CombatantView> targets)
    {
        StatusEffectType = type;
        Targets = targets;
    }
}
