using SerializeReferenceEditor;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ContainsUnitsLTM : LaneTargetMode
{

    [SerializeReference, SR] private CombatantTargetMode _combatantTargetMode = new ManualTargetCTM();

    public override List<LaneView> GetTargets(EffectContext context)
    {
        List<CombatantView> targets = _combatantTargetMode.GetTargets(context);

        if (targets.Count == 0)
            return new();

        List<LaneView> allLanes = BoardSystem.Instance.GetAllLanes().FindAll(l => targets.Any(t => t.Lane == l));

        return allLanes;
    }

    public override List<LaneView> AllPossibleTargets(EffectContext context, Card card = null)
    {
        List<CombatantView> targets = _combatantTargetMode.AllPossibleTargets(context, card);

        if (targets.Count == 0)
            return new();

        List<LaneView> allLanes = BoardSystem.Instance.GetAllLanes().FindAll(l => targets.Any(t => t.Lane == l));

        return allLanes;
    }
}
