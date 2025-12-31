using SerializeReferenceEditor;
using System.Collections.Generic;
using UnityEngine;

public class AllLTM : LaneTargetMode
{
    [SerializeReference, SR] private List<LaneFilter> _filters;

    public override List<LaneView> GetTargets(EffectContext context)
    {
        List<LaneView> lanes = BoardSystem.Instance.GetAllLanes();
        
        if(_filters.Count > 0)
            lanes = new(lanes.ApplyFilters(_filters));
        
        return lanes;
    }
}
