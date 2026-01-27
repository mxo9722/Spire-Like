using SerializeReferenceEditor;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FrontFoeInLaneCTM : CombatantTargetMode
{

    [SerializeReference, SR] private LaneTargetMode _laneTargetMode = new CurrentLTM();

    public override List<CombatantView> GetTargets(EffectContext context)
    {
        List<LaneView> laneViews = _laneTargetMode.GetTargets(context);

        if (laneViews.Count > 0)
        {
            laneViews.RemoveAll(l => l == null);

            if (context.Caster is NPCView npc && npc.IsEvil)
            {
                var select = laneViews.Select(l => l.HeroView);
                var where = select.Where(e => e != null);
                return where.ToList();
            }

            return laneViews.Select(l => l.FrontEnemyView()).Where(e => e != null).ToList();
        }

        return new();
    }

    public override NPCTargetTypes GetTargetIntent()
    {
        return NPCTargetTypes.FRONT_FOE;
    }
}