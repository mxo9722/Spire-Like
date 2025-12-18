using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnSystem : Singleton<BurnSystem>
{
    [SerializeField] private GameObject _burnVFX;

    private void OnEnable()
    {
        ActionSystem.AttachPerformer<ApplyBurnGA>(ApplyBurnPerformer);
    }

    private void OnDisable()
    {
        ActionSystem.DetachPerformer<ApplyBurnGA>();
    }

    private IEnumerator ApplyBurnPerformer(ApplyBurnGA applyBurnGA)
    {
        List<CombatantView> targets = applyBurnGA.Targets;

        foreach (CombatantView target in targets)
        {
            Instantiate(_burnVFX, target.transform.position, Quaternion.identity);

            int burnStacks = target.GetStatusEffectStacks(StatusEffectType.BURN);

            target.Damage(burnStacks, true);
            target.AddStatusEffect(StatusEffectType.BURN, -1);
        }

        yield return new WaitForSeconds(1f);
    }
}
