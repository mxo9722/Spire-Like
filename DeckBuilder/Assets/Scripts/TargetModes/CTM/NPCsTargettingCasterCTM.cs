using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPCsTargettingCasterCTM : CombatantTargetMode
{
    public override List<CombatantView> GetTargets(EffectContext context)
    {
        List<NPCView> units = BoardSystem.Instance.GetAllFoes(context.Caster).FindAll(unit => unit is NPCView).ConvertAll(unit => (NPCView) unit);

        units.RemoveAll(unit => !unit.IsAttackingUnit(context.Caster,context));

        return new(units.Cast<CombatantView>());
    }
}
