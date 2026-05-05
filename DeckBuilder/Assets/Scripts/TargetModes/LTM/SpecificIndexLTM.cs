using SerializeReferenceEditor;
using System.Collections.Generic;
using UnityEngine;

public class SpecificIndexLTM : LaneTargetMode
{

    [SerializeReference, SR] private LaneTargetMode _laneCollection;
    [SerializeField, Min(0)] private int _index;

    public override List<LaneView> GetTargets(EffectContext context)
    {
        List<LaneView> list = _laneCollection.GetTargets(context);

        if (_index >= list.Count)
            return new();

        LaneView hold = list[_index];
        list.Clear();
        list.Add(hold);

        return list;
    }

    public override List<LaneView> GetTargetsTrivial(EffectContext context)
    {
        List<LaneView> list = _laneCollection.GetTargetsTrivial(context);

        if (_index >= list.Count)
            return new();

        LaneView hold = list[_index];
        list.Clear();
        list.Add(hold);

        return list;
    }
}
