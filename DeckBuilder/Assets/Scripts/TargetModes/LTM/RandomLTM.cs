using SerializeReferenceEditor;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RandomLTM : LaneTargetMode
{

    [SerializeField] private int _count = 1;
    [field: SerializeReference, SR] public List<LaneFilter> Filters { get; private set; } = new();

    public override bool IsRandom => true;

    public override List<LaneView> GetTargets(EffectContext context)
    {
        return GetTargets(context,RNG.Random);
    }
    
    public override List<LaneView> GetTargetsTrivial(EffectContext context)
    {
        return GetTargets(context,RNG.TrivialRandom);
    }

    private List<LaneView> GetTargets(EffectContext context, System.Random random)
    {
        List<LaneView> all = new(BoardSystem.Instance.GetAllLanes());

        if (Filters.Count > 0)
            all.RemoveAll(l => !Filters.TrueForAll(f => f.TestTarget(context, l)));

        if (_count >= all.Count)
            return all;

        int removesLeft = all.Count - _count;

        for (int i = 0; i < removesLeft; i++)
        {
            all.RemoveAt(random.Next(all.Count));
        }

        return all;
    }

    public override List<LaneView> AllPossibleTargets(EffectContext context, Card card = null)
    {
        List<LaneView> possible = BoardSystem.Instance.GetAllLanes();

        if(context.Caster == null)
        {
            List<LaneView> ret = new();

            foreach(HeroView hero in HeroSystem.Instance.HeroViews)
            {
                EffectContext heroContext = new(hero, context.TargetLane, context.TargetCombatant, context.PlayedCard);
                ret.AddRange(possible.ApplyFilters(Filters, heroContext));
            }

            return ret.Distinct().ToList();
        }


        possible = new(possible.ApplyFilters(Filters, context));

        return possible;
    }

}
