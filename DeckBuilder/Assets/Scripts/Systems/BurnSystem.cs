using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnSystem : Singleton<BurnSystem>
{
    [SerializeField] private GameObject _burnVFX;

    private void OnEnable()
    {
        ActionSystem.AttachPerformer<InvokeBurnGA>(ApplyBurnPerformer);
        ActionSystem.AttachPerformer<TransferHeatGA>(TransferHeatPerformer);
    }

    private void OnDisable()
    {
        ActionSystem.DetachPerformer<InvokeBurnGA>();
        ActionSystem.AttachPerformer<TransferHeatGA>(TransferHeatPerformer);
    }

    private IEnumerator ApplyBurnPerformer(InvokeBurnGA applyBurnGA)
    {
        List<CombatantView> targets = applyBurnGA.Targets;

        foreach (CombatantView target in targets)
        {
            if (target == null)
                continue;

            Instantiate(_burnVFX, target.transform.position, Quaternion.identity);

            int burnStacks = target.GetStatusEffectStacks(StatusEffect.BURN);

            target.Damage(burnStacks, true);
            target.AddStatusEffect(StatusEffectSystem.GetDictionaryEntry(StatusEffect.BURN), -1);
        }

        yield return new WaitForSeconds(1f);
    }

    private IEnumerator TransferHeatPerformer(TransferHeatGA transferHeatGA)
    {
        List<GameAction> gameActions = new();

        foreach (CombatantView target in transferHeatGA.Targets)
        {
            int stacks = target.GetStatusEffectStacks(StatusEffect.HEAT);
            gameActions.Add(new AddStatusEffectGA(StatusEffectSystem.GetDictionaryEntry(StatusEffect.BURN), stacks, new() { target }));
            gameActions.Add(new RemoveAllStatusEffectGA(StatusEffectSystem.GetDictionaryEntry(StatusEffect.HEAT), new() { target }));
        }


        MultipleGameActionsGA multipleGameActionsGA = new(gameActions);
        ActionSystem.Instance.AddReaction(multipleGameActionsGA);
        DynamicViewsSystem.Instance.UpdateDynamicValues();

        yield return null;
    }
}
