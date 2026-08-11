using SerializeReferenceEditor;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SEStacksSOQ : Quantity
{
    [SerializeReference, SR] private CombatantTargetMode _targetMode = new CasterCTM();
    [SerializeField] private StatusEffectData _data;

    public override int GetAmount(EffectContext effectContext)
    {
        List<CombatantView> targets = _targetMode.GetTargets(effectContext);
        targets = targets.Where(t => t != null).ToList();

        return targets.Sum(t => t.GetStatusEffectStacks(_data.Info));
    }

    public override int GetStaticAmount()
    {
        List<CombatantView> targets = _targetMode.GetTargets(new());

        return targets.Sum(t => t.GetStatusEffectStacks(_data.Info));
    }
}
