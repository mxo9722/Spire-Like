using System.Collections.Generic;
using UnityEngine;

public class DealDamageGA : GameAction, IHaveCaster
{
    public int Amount { get; private set; }
    public List<CombatantView> Targets { get; private set; }

    public CombatantView Caster { get; private set; }
    public bool IsAttack { get; private set; }

    public DealDamageGA(int amount, CombatantView target, CombatantView caster, bool isAttack = true)
    {
        Amount = amount;
        Targets = new();
        Targets.Add(target);
        Caster = caster;
        IsAttack = isAttack;
    }
    
    public DealDamageGA(int amount, List<CombatantView> targets, CombatantView caster, bool isAttack = true)
    {
        Amount = amount;
        Targets = new(targets);
        Caster = caster;
        IsAttack = isAttack;
    }

    public void SetDamage(int damage)
    {
        Amount = damage;
    }
}
