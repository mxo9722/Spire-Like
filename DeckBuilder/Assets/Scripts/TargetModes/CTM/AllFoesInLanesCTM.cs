using SerializeReferenceEditor;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AllFoesInLanesCTM : CombatantTargetMode
{

    [SerializeReference, SR] private LaneTargetMode _laneTargetMode = new CurrentLTM();

    public override List<CombatantView> GetTargets(EffectContext context)
    {
        List<LaneView> laneViews = _laneTargetMode.GetTargets(context);

        List<CombatantView> foes;
        
        if(context.Caster is NPCView npc && npc.IsEvil)
            foes = laneViews.SelectMany(l => l.HeroViews).ToList();
        else
            foes = laneViews.SelectMany(l => l.EnemyViews).Cast<CombatantView>().ToList();

        return foes;
    }

    public override NPCTargetTypes GetTargetIntent()
    {
        return NPCTargetTypes.WHOLE_LANE;
    }
}
