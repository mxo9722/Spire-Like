using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PerformEffectsGA : GameAction
{
    public Effect Effect { get; set; }
    public List<CombatantView> CombatantTargets { get; set; } = new();
    public List<LaneView> LaneTargets { get; set; } = new();
    public List<Card> CardTargets { get; set; } = new();

    public PerformEffectsGA(Effect effect)
    {
        Effect = effect;
    }

    public PerformEffectsGA(Effect effect, List<CombatantView> targets)
    {
        Effect = effect;
        CombatantTargets = targets == null ? new() : targets;
    }
    
    public PerformEffectsGA(Effect effect, CombatantView target)
    {
        Effect = effect;
        CombatantTargets = target == null ? new() : new() { target };
    }
    
    public PerformEffectsGA(Effect effect, List<LaneView> targets)
    {
        Effect = effect;
        LaneTargets = targets == null ? new() : targets;

    }
    
    public PerformEffectsGA(Effect effect, LaneView target)
    {
        Effect = effect;
        LaneTargets = target == null ? new() : new() { target };
    }
    
    public PerformEffectsGA(Effect effect, List<Card> targets)
    {
        Effect = effect;
        CardTargets = targets == null ? new() : targets;

    }
    
    public PerformEffectsGA(Effect effect, Card target)
    {
        Effect = effect;
        CardTargets = target == null ? new() : new() { target };
    }
}
