using System.Collections.Generic;
using UnityEngine;

public class AttackHeroEffect : Effect
{
    [field: SerializeField, Min(0)] public int Damage { get; private set; } = 0;

    public override GameAction GetGameAction(List<CombatantView> targets, CombatantView caster)
    {
        AttackHeroGA attackHeroGA = new AttackHeroGA(Damage, targets, (EnemyView)caster);
        return attackHeroGA;
    }
}
