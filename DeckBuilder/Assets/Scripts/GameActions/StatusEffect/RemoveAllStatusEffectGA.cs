using System.Collections.Generic;
using UnityEngine;

public class RemoveAllStatusEffectGA : GameAction
{

    public StatusEffectInfo StatusEffectInfo { get; private set; }
    public List<CombatantView> Targets { get; private set; }

    public RemoveAllStatusEffectGA(StatusEffectInfo statusEffectInfo, List<CombatantView> targets)
    {
        StatusEffectInfo = statusEffectInfo;
        Targets = targets;
    }
}
