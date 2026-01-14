using SerializeReferenceEditor;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectStacksQ : Quantity
{
    [SerializeReference, SR] private CombatantTargetMode _target = new CasterCTM();
    [SerializeField] private StatusEffectType _type;

    public override int GetAmount(EffectContext effectContext)
    {
        List<CombatantView> targets = _target.GetTargets(effectContext);
        targets.RemoveAll(t => t == null);

        if (targets.Count == 0)
            return 0;

        return targets[0].GetStatusEffectStacks(_type);
    }

    public override int GetStaticAmount()
    {
        return GetAmount(new());
    }
}
