using UnityEngine;

public class TargIsEvilFilter : CombatantFilter
{
    protected override bool TargetIsValid(EffectContext context, CombatantView target)
    {
        return (target is NPCView npc && npc.IsEvil);
    }
}
