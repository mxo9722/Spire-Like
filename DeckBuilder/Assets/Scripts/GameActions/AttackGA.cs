using System.Collections.Generic;
using UnityEngine;

public class AttackGA : GameAction, IHaveCaster
{
    public int Amount { get; private set; }
    public List<CombatantView> Targets { get; private set; }

    public CombatantView Caster { get; private set; }

    public AttackGA(int amount, CombatantView target, CombatantView caster)
    {
        Amount = amount;
        Targets = new();
        Targets.Add(target);
        Caster = caster;
    }

    public AttackGA(int amount, List<CombatantView> targets, CombatantView caster)
    {
        Amount = amount;
        Targets = new(targets);
        Caster = caster;
    }
}