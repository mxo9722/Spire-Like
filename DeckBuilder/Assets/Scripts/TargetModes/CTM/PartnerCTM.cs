using System.Collections.Generic;
using UnityEngine;

public class PartnerCTM : CombatantTargetMode
{
    public override List<CombatantView> GetTargets(EffectContext context)
    {
        if (context.Caster == null)
            return new();

        List<CombatantView> allies = BoardSystem.Instance.GetAllAllies(context.Caster);

        return allies;
    }
}
