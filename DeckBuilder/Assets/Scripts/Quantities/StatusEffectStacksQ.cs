using SerializeReferenceEditor;
using UnityEngine;

public class StatusEffectStacksQ : Quantity
{
    [SerializeReference, SR] private CombatantTargetMode _target = new CasterCTM();
    [SerializeField] private StatusEffectType _type;

    public override int GetAmount(EffectContext effectContext)
    {
        var targets = _target.GetTargets(effectContext);

        if (targets.Count == 0)
            return 0;

        return targets[0].GetStatusEffectStacks(_type);
    }

    public override int GetStaticAmount()
    {
        return GetAmount(new());
    }
}
