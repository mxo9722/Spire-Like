using System.Collections.Generic;
using UnityEngine;

public class CurrentLTM : LaneTargetMode
{
    public override List<LaneView> GetTargets(EffectContext context)
    {
        if(context.Caster != null)
            return new() { BoardSystem.Instance.GetCurrentLaneView(context.Caster) };
        

        if(context.PlayedCard != null)
        {
            List<LaneView> lanes = new();

            foreach(HeroView hero in HeroSystem.Instance.HeroViews)
            {
                lanes.AddRange(GetTargets(new(hero, context.TargetLane, context.TargetCombatant, context.PlayedCard)));
            }

            return lanes;
        }

        return new();
    }
}
