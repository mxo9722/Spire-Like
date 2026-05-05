using System.Collections.Generic;
using UnityEngine;

public class MultiAttackHeroGA : GameAction
{
    public EffectContext Context { get; private set; }
    public CombatantView Caster => Context.Caster;

    public List<CombatantView> Targets { get; private set; }

    public int AttackTimes { get; private set; }
    public int Damage { get; private set; }

    public bool IndirectReduction { get; private set; } = true;

    public string UnblockedKey { get; private set; } = "";
    public string OverkillKey { get; private set; } = "";
    public string OnHitKey { get; private set; } = "";
    public string HitCountKey { get; private set; } = "";

    public MultiAttackHeroGA(int attackTimes, int damage, List<CombatantView> targets, EffectContext context, bool indirectReduction, string unblockedKey, string overkillKey, string onHitKey, string hitCountKey)
    {
        AttackTimes = attackTimes;
        Damage = damage;
        Targets = targets;
        Context = context;
        IndirectReduction = indirectReduction;
        UnblockedKey = unblockedKey;
        OverkillKey = overkillKey;
        OnHitKey = onHitKey;
        HitCountKey = hitCountKey;
    }
}
