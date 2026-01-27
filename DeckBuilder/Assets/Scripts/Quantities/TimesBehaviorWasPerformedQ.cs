using UnityEngine;
using System.Linq;
public class TimesBehaviorWasPerformedQ : Quantity
{

    [SerializeField] private string _behaviorName;

    public override int GetAmount(EffectContext effectContext)
    {
        if(effectContext.Caster is NPCView npc)
        {
            return npc.PreviousActions.Count(pa => pa.Name == _behaviorName);
        }

        return 0;
    }

    public override int GetStaticAmount()
    {
        return 0;
    }
}
