using SerializeReferenceEditor;
using System.Linq;
using UnityEngine;

public class MaxHealthQ : Quantity
{
    [SerializeReference, SR] private HeroTargetMode TargetMode;

    public override int GetAmount(EffectContext effectContext)
    {
        var targets = TargetMode.GetTargets(effectContext);

        var target = targets.First();
        if (target == null)
            return 0;
        
        return target.MaxHealth;
    }

    public override int GetStaticAmount()
    {
        return RunSystem.Instance.Hero1.MaxHealth;
    }
}
