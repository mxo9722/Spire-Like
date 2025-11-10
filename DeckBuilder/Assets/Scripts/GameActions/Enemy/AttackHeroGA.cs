using System.Collections.Generic;
using UnityEngine;

public class AttackHeroGA : GameAction, IHaveCaster
{
    public EnemyView Attacker { get; private set; }
    public CombatantView Caster { get => Attacker; }

    public List<CombatantView> Targets { get; private set; }
    public int Damage { get; private set; }

    public AttackHeroGA(int damage, List<CombatantView> targets, EnemyView attacker)
    {
        Damage = damage;
        Targets = targets;
        Attacker = attacker;
    }
}
