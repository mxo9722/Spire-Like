using System.Collections.Generic;
using UnityEngine;

public class AttackHeroGA : GameAction, IHaveCaster
{
    public EffectContext Context { get; private set; }

    public CombatantView Caster => Context.Caster;

    public List<CombatantView> Targets { get; private set; }
    public int Damage { get; private set; }


    public string UnblockedKey { get; private set; } = "";
    public string OverkillKey { get; private set; } = "";
    public string OnHitKey { get; private set; } = "";

    public AttackHeroGA(int damage, List<CombatantView> targets, EffectContext context, string unblockedKey, string overkillKey, string onHitKey)
    {
        Damage = damage;
        Targets = targets;
        Context = context;
        UnblockedKey = unblockedKey;
        OverkillKey = overkillKey;
        OnHitKey = onHitKey;
    }
}
