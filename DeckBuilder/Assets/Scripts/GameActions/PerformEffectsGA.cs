using System.Collections.Generic;
using UnityEngine;

public class PerformEffectsGA : GameAction
{
    public Effect Effect { get; set; }
    public List<CombatantView> Targets { get; set; }

    public PerformEffectsGA(Effect effect, List<CombatantView> targets)
    {
        Effect = effect;
        Targets = targets == null ? new() : targets;
    }
    
    public PerformEffectsGA(Effect effect, CombatantView target)
    {
        Effect = effect;
        Targets = target == null ? new() : new() { target };
    }
}
