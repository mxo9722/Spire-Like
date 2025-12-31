using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PerformEffectsGA : GameAction, IHaveCaster
{
    public Effect Effect { get; set; }
    public List<CombatantView> CombatantTargets { get; set; } = new();
    public List<LaneView> LaneTargets { get; set; } = new();
    public List<Card> CardTargets { get; set; } = new();

    public EffectContext Context { get; private set; }

    public CombatantView Caster => Context.Caster;

    public PerformEffectsGA(EffectContext context, Effect effect)
    {
        Effect = effect;
        Context = context;
    }

    public PerformEffectsGA(EffectContext context, Effect effect, List<CombatantView> targets)
    {
        Context = context;
        Effect = effect;
        CombatantTargets = targets == null ? new() : targets;
    }
    
    public PerformEffectsGA(EffectContext context, Effect effect, CombatantView target)
    {
        Context = context;
        Effect = effect;
        CombatantTargets = target == null ? new() : new() { target };
    }
    
    public PerformEffectsGA(EffectContext context, Effect effect, List<LaneView> targets)
    {
        Context = context;
        Effect = effect;
        LaneTargets = targets == null ? new() : targets;

    }
    
    public PerformEffectsGA(EffectContext context, Effect effect, LaneView target)
    {
        Context = context;
        Effect = effect;
        LaneTargets = target == null ? new() : new() { target };
    }
    
    public PerformEffectsGA(EffectContext context, Effect effect, List<Card> targets)
    {
        Context = context;
        Effect = effect;
        CardTargets = targets == null ? new() : targets;

    }
    
    public PerformEffectsGA(EffectContext context, Effect effect, Card target)
    {
        Context = context;
        Effect = effect;
        CardTargets = target == null ? new() : new() { target };
    }

    public GameAction GetGameAction(EffectContext context)
    {
        return Effect.GetGameAction(context, CombatantTargets, LaneTargets, CardTargets);
    }
}
