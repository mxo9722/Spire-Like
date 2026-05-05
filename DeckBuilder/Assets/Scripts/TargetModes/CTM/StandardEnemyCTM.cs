using System.Collections.Generic;
using UnityEngine;

public class StandardEnemyCTM : CombatantTargetMode
{
    public override List<CombatantView> GetTargets(EffectContext context)
    {
        CombatantView caster = context.Caster;

        if (caster == null) return new();

        if (caster.Lane.HeroView != null) return new() { context.Caster.Lane.HeroView };

        List<HeroView> heroes = new(HeroSystem.Instance.HeroViews);

        heroes.Sort(
            (a, b) =>
            {
                int value = 0;

                int distA = a.GetLaneDistance(caster);
                int distB = b.GetLaneDistance(caster);

                value = distA - distB;
                value *= 2;
                value += Mathf.Clamp(a.CurrentHealth - b.CurrentHealth, -1, 1);

                return value;
            }
            
            );

        return new() { heroes[0] };
    }
}
