using System.Collections.Generic;
using UnityEngine;

public class TransferHeatGA : GameAction
{
    public List<CombatantView> Targets { get; private set; }
    public EffectContext Context { get; private set; }

    public TransferHeatGA(List<CombatantView> targets, EffectContext context)
    {
        Targets = targets;
        Context = context;
    }
}
