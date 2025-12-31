using SerializeReferenceEditor;
using System.Collections.Generic;
using UnityEngine;

public class RandomCTM : CombatantTargetMode
{
    [SerializeField, Min(1)] private int _targetCount = 1;

    [field: SerializeReference, SR] public List<CombatantFilter> Filters { get; private set; } = new();

    public override bool IsRandom => true;

    public override List<CombatantView> GetTargets(EffectContext context)
    {
        return GetTargets(context, RNG.Random);
    }

    public override List<CombatantView> GetTargetsTrivial(EffectContext context)
    {
        return GetTargets(context, RNG.TrivialRandom);
    }

    private List<CombatantView> GetTargets(EffectContext context, System.Random random)
    {
        List<CombatantView> npcs = new(BoardSystem.Instance.BoardView.GetAllCombatants().ApplyFilters(Filters, context));

        if (npcs.Count == 0)
            return new();
        else if (npcs.Count <= _targetCount)
            return npcs;

        List<CombatantView> targets = new();

        for (int i = 0; i < _targetCount; i++)
        {
            int index = RNG.Random.Next(npcs.Count);
            CombatantView target = npcs[index];
            npcs.RemoveAt(index);
            targets.Add(target);
        }

        return targets;
    }

    public override List<CombatantView> AllPossibleTargets(EffectContext context, Card card = null)
    {
        List<CombatantView> possible = BoardSystem.Instance.BoardView.GetAllCombatants();

        possible = new(possible.ApplyFilters(Filters, context));

        return possible;
    }
}
