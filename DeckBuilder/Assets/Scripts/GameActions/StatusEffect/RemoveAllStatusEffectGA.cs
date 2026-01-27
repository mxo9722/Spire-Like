using System.Collections.Generic;
using UnityEngine;

public class RemoveAllStatusEffectGA : GameAction
{
    public StatusEffect StatusEffectType { get; private set; }
    public List<CombatantView> Targets { get; private set; }

    public RemoveAllStatusEffectGA(StatusEffect type, List<CombatantView> targets)
    {
        StatusEffectType = type;
        Targets = targets;
    }
}
