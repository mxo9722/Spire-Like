using SerializeReferenceEditor;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AdjacentTargetsCTM : CombatantTargetMode
{
    [SerializeReference, SR] private CombatantTargetMode _baseTargets;

    public override List<CombatantView> GetTargets(EffectContext context)
    {
        List<CombatantView> pTargets = _baseTargets.GetTargets(context);
        List<CombatantView> adjacent = new();

        foreach(CombatantView targ in pTargets)
        {
            adjacent.AddRange(GetAdjacents(targ));
        }

        adjacent.RemoveAll(r => r == null);
        adjacent = new(adjacent.Distinct());
        adjacent.RemoveAll(r => pTargets.Contains(r));

        return adjacent;
    }

    private List<CombatantView> GetAdjacents(CombatantView pTarget)
    {
        if (pTarget == null)
            return new();

        LaneView lane = BoardSystem.Instance.GetCurrentLaneView(pTarget);
        int index = pTarget.Slot.Index;
        bool isEvil = pTarget is NPCView npc && npc.IsEvil;

        CombatantView left = GetCombatantAtIndex(lane, index - 1, isEvil);
        CombatantView right = GetCombatantAtIndex(lane, index + 1, isEvil);

        LaneView upLane = GetLaneAtIndex(lane.Board, lane.Index - 1);
        LaneView downLane = GetLaneAtIndex(lane.Board, lane.Index + 1);


        CombatantView up = GetCombatantAtIndex(upLane,index,isEvil);
        CombatantView down = GetCombatantAtIndex(downLane, index, isEvil);

        return new() { left, right, up, down };
    }

    private LaneView GetLaneAtIndex(BoardView board, int index)
    {
        if (index < 0) return null;
        List<LaneView> lanes = board.GetAllLanes();
        if (lanes.Count <= index) return null;

        return lanes[index];
    }

    private CombatantView GetCombatantAtIndex(LaneView lane, int index, bool isEvil)
    {
        if (lane == null) return null;
        if (index < 0) return null;
        SlotView[] slots = isEvil ? lane.EnemySlots : lane.HeroSlots;
        if (slots.Length <= index) return null;

        SlotView slot = slots[index];

        if (slot.Combatant == null)
            return null;

        return slot.Combatant;
    }
}
