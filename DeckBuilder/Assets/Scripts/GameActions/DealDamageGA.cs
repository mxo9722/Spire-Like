using System;
using System.Collections.Generic;
using UnityEngine;

public class DealDamageGA : SimulatedGameAction, IHaveCaster
{
    public int Amount { get; private set; }
    public List<CombatantView> Targets { get; private set; }

    public EffectContext Context { get; private set; }

    public CombatantView Caster => Context.Caster;
    public bool IsAttack { get; private set; }

    public int UnblockedDamage { get; private set; }
    public int Overkill { get; private set; }

    public string UnblockedKey { get; private set; } = "";
    public string OverkillKey { get; private set; } = "";

    public DealDamageGA(int amount, CombatantView target, EffectContext context, bool isAttack = true)
    {
        Amount = amount;
        Targets = new() { target };
        Context = context;
        IsAttack = isAttack;
    }
    
    public DealDamageGA(int amount, List<CombatantView> targets, EffectContext context, bool isAttack = true)
    {
        Amount = amount;
        Targets = new(targets);
        Context = context;
        IsAttack = isAttack;
    }

    public void SetDamage(int damage)
    {
        Amount = damage;
    }

    public void SetUnblockedDamage(int amount)
    {
        UnblockedDamage = amount;
        if (!string.IsNullOrWhiteSpace(UnblockedKey))
            Context.SetData(UnblockedKey, amount);
    }
    
    public void SetOverkill(int amount)
    {
        Overkill = amount;
        if (!string.IsNullOrWhiteSpace(OverkillKey))
            Context.SetData(OverkillKey, amount);
    }

    public void SetUnblockedKey(string key)
    {
        UnblockedKey = key;
    }
    
    public void SetOverkillKey(string key)
    {
        OverkillKey = key;
    }

    public override void SimulatedPerform(EffectContext context)
    {
        foreach(CombatantView target in Targets)
        {
            if (!string.IsNullOrWhiteSpace(UnblockedKey))
            {
                int unblockedDamage = Amount;

                if (IsAttack)
                    unblockedDamage = DamageSystem.GetDamageFromAttack(Amount, context, new() { target });

                unblockedDamage = target.GetStatusEffectStacks(StatusEffect.BLOCK) - unblockedDamage;

                Context.SetData(UnblockedKey, unblockedDamage);
            }

            if (!string.IsNullOrWhiteSpace(OverkillKey))
            {
                int overkill = Amount;

                if (IsAttack)
                    overkill = DamageSystem.GetDamageFromAttack(Amount, context, new() { target });

                overkill = target.GetStatusEffectStacks(StatusEffect.BLOCK) + target.CurrentHealth - overkill;

                Context.SetData(OverkillKey, overkill);
            }
        }
    }
}
