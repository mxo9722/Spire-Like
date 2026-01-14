using System.Collections.Generic;
using UnityEngine;

public class AllNPCsOfTypeCTM : CombatantTargetMode
{

    [SerializeField] private NPCData _npcType;

    public override List<CombatantView> GetTargets(EffectContext context)
    {
        List<CombatantView> targets = BoardSystem.Instance.GetAllCombatants();

        targets.RemoveAll(t => !(t is NPCView npc && npc.Data == _npcType));

        return targets;
    }
}
