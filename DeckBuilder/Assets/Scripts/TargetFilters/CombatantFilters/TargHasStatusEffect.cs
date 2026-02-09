using SerializeReferenceEditor;
using UnityEngine;

public class TargHasStatusEffect : CombatantFilter
{
    [SerializeField] private StatusEffect _statusEffectType;

    [SerializeField] private NumberCompare _is = NumberCompare.GREATER_THAN;
    [SerializeField] private bool _orEqualTo = false;
    [SerializeReference, SR] private Quantity _b = new SetQ(0);


    protected override bool TargetIsValid(EffectContext context, CombatantView target)
    {
        int stacks = target.GetStatusEffectStacks(_statusEffectType);
        int b = _b.GetAmount(context);

        if (stacks == b && _orEqualTo)
            return true;

        switch (_is)
        {
            case NumberCompare.LESS_THAN:
                return stacks < b;
            case NumberCompare.EQUAL_TO:
                return stacks == b;
            case NumberCompare.GREATER_THAN:
                return stacks > b;
        }

        return false;
    }
}
