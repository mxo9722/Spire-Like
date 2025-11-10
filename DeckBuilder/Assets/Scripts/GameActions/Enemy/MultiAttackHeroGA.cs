using System.Collections.Generic;
using UnityEngine;

public class MultiAttackHeroGA : GameAction
{
    public EnemyView Attacker { get; private set; }
    public CombatantView Caster { get => Attacker; }

    public List<CombatantView> Targets { get; private set; }

    [Min(2)] public int AttackTimes { get; private set; }
    [Min(0)] public int Damage { get; private set; }

    public MultiAttackHeroGA(int attackTimes, int damage, List<CombatantView> targets, EnemyView attacker)
    {
        AttackTimes = attackTimes;
        Damage = damage;
        Targets = targets;
        Attacker = attacker;
    }
}
