using UnityEngine;

public class AttackHeroGA : GameAction, IHaveCaster
{
    public EnemyView Attacker { get; private set; }

    public CombatantView Caster { get => Attacker; }

    public int Damage { get; private set; }

    public AttackHeroGA(int damage, EnemyView attacker)
    {
        Damage = damage;
        Attacker = attacker;
    }
}
