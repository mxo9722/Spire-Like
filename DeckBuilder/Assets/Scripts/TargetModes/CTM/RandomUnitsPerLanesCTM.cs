using SerializeReferenceEditor;
using System.Collections.Generic;
using UnityEngine;

public class RandomUnitsPerLanesCTM : CombatantTargetMode
{
    [SerializeReference, SR] private LaneTargetMode _lanesTargetMode;
    [SerializeReference, SR] private List<CombatantFilter> _combatantFilters;

    [SerializeReference, SR] private Quantity _countPerLane = new SetQ(1);

    public override bool IsRandom => true;

    public override List<CombatantView> GetTargets(EffectContext context)
    {
        List<LaneView> lanes = _lanesTargetMode.GetTargets(context);
        List<CombatantView> combatantViews = new();

        foreach (LaneView lane in lanes)
        {
            List<CombatantView> validTargets = lane.GetAllUnits();
            validTargets = new(validTargets.ApplyFilters(_combatantFilters, context));

            if (validTargets.Count == 0)
                continue;

            int count = _countPerLane.GetAmount(context);

            if (count > validTargets.Count)
                combatantViews.AddRange(validTargets);
            else
            {
                for (int i = 0; i < count; i++)
                {
                    int index = RNG.Random.Next(validTargets.Count);
                    combatantViews.Add(validTargets[index]);
                    validTargets.RemoveAt(index);
                }
            }
        }

        return combatantViews;
    }

    public override List<CombatantView> GetTargetsTrivial(EffectContext context)
    {
        List<LaneView> lanes = _lanesTargetMode.GetTargetsTrivial(context);
        List<CombatantView> combatantViews = new();

        foreach (LaneView lane in lanes)
        {
            List<CombatantView> validTargets = lane.GetAllUnits();
            validTargets = new(validTargets.ApplyFilters(_combatantFilters, context));

            if (validTargets.Count == 0)
                continue;

            int count = _countPerLane.GetAmount(context);

            if (count > validTargets.Count)
                combatantViews.AddRange(validTargets);
            else
            {
                for (int i = 0; i < count; i++)
                {
                    int index = RNG.TrivialRandom.Next(validTargets.Count);
                    combatantViews.Add(validTargets[index]);
                    validTargets.RemoveAt(index);
                }
            }
        }

        return combatantViews;
    }


    public override List<CombatantView> AllPossibleTargets(EffectContext context, Card card = null)
    {
        List<LaneView> lanes = _lanesTargetMode.GetTargetsTrivial(context);
        List<CombatantView> combatantViews = new();

        foreach (LaneView lane in lanes)
        {
            List<CombatantView> validTargets = lane.GetAllUnits();
            validTargets = new(validTargets.ApplyFilters(_combatantFilters, context));
            combatantViews.AddRange(validTargets);
        }

        return combatantViews;
    }
}
