using System.Collections.Generic;
using UnityEngine;

public class MultiplyStatusEffectGA : GameAction
{
    public StatusEffectInfo StatusEffect { get; private set; }
    public float Multiplier { get; private set; }
    public List<CombatantView> Targets { get; private set; }

    public MultiplyStatusEffectGA(StatusEffectInfo statusEffect, float multiplier, List<CombatantView> targets)
    {
        StatusEffect = statusEffect;
        Multiplier = multiplier;
        Targets = targets;
    }
}
