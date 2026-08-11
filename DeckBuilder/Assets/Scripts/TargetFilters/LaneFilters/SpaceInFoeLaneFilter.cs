using System.Linq;
using UnityEngine;

public class SpaceInFoeLaneFilter : LaneFilter
{
    protected override bool TargetIsValid(EffectContext context, LaneView target)
    {
        if(context.Caster is NPCView npc && npc.IsEvil)
        {
            return target.HeroSlot.Combatant == null;
        }
        return target.EnemySlots.Any(s => s.Combatant == null);
    }
}
