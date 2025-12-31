using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnSystem : Singleton<BurnSystem>
{
    [SerializeField] private GameObject _burnVFX;

    private void OnEnable()
    {
        ActionSystem.AttachPerformer<InvokeBurnGA>(ApplyBurnPerformer);
    }

    private void OnDisable()
    {
        ActionSystem.DetachPerformer<InvokeBurnGA>();
    }

    private IEnumerator ApplyBurnPerformer(InvokeBurnGA applyBurnGA)
    {
        List<CombatantView> targets = applyBurnGA.Targets;

        foreach (CombatantView target in targets)
        {
            if (target == null)
                continue;

            Instantiate(_burnVFX, target.transform.position, Quaternion.identity);

            int burnStacks = target.GetStatusEffectStacks(StatusEffectType.BURN);

            target.Damage(burnStacks, true);
            target.AddStatusEffect(StatusEffectType.BURN, -1);
        }

        yield return new WaitForSeconds(1f);
    }
}
