using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AttackHeroEffect : CombatantTargetEffect
{
    [field: SerializeField, Min(0)] public int Damage { get; private set; } = 0;

    protected override GameAction GetGameAction(CombatantView caster, List<CombatantView> combatantTargets)
    {
        AttackHeroGA attackHeroGA = new AttackHeroGA(Damage, combatantTargets, (EnemyView)caster);
        return attackHeroGA;
    }
}
