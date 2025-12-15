using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StatusEffectSystem : Singleton<StatusEffectSystem>
{

    [SerializeField] private ParticleSystem _defendVFX;
    [SerializeField] private StatusEffectsData _statusEffectData;

    public const float FRAIL_MULITPLIER = 0.75f;

    private void OnEnable()
    {
        ActionSystem.AttachPerformer<AddStatusEffectGA>(AddStatusEffectPerformer);
        ActionSystem.AttachPerformer<RemoveAllStatusEffectGA>(RemoveAllStatusEffectPerformer);

        ActionSystem.SubscribeReaction<AddStatusEffectGA>(PreStatusEffectReaction, ReactionTiming.PRE);
    }

    private void OnDisable()
    {
        ActionSystem.DetachPerformer<AddStatusEffectGA>();
        ActionSystem.DetachPerformer<RemoveAllStatusEffectGA>();

        ActionSystem.UnsubscribeReaction<AddStatusEffectGA>(PreStatusEffectReaction, ReactionTiming.PRE);
    }

    private IEnumerator AddStatusEffectPerformer(AddStatusEffectGA addStatusEffectGA)
    {
        float waitTime = 0;

        foreach (CombatantView target in addStatusEffectGA.Targets)
        {
            if (target.CurrentHealth <= 0)
                continue;
            switch (addStatusEffectGA.StatusEffectType)
            {
                default:
                    if (addStatusEffectGA.StackCount > 0)
                    {
                        ParticleSystem effect = Instantiate(_defendVFX, target.transform);

                        StatusEffectInfo data = _statusEffectData.Map[addStatusEffectGA.StatusEffectType];

                        effect.textureSheetAnimation.SetSprite(0, data.Sprite);

                        effect.transform.localPosition = Vector3.zero;
                        effect.transform.localScale = Vector3.one;
                        waitTime = 0.75f;
                    }

                    break;
            }
        }

        if (waitTime > 0)
            yield return new WaitForSeconds(waitTime);

        foreach (CombatantView target in addStatusEffectGA.Targets)
        {
            if (target.CurrentHealth <= 0)
                continue;
            target.AddStatusEffect(addStatusEffectGA.StatusEffectType, addStatusEffectGA.StackCount);
            ///TODO: Add a special effect

            yield return null;
        }

        DynamicViewsSystem.Instance.UpdateDynamicValues();
    }

    private IEnumerator RemoveAllStatusEffectPerformer(RemoveAllStatusEffectGA removeAllStatusEffect)
    {
        foreach(CombatantView target in removeAllStatusEffect.Targets)
        {
            int stackCount = target.GetStatusEffectStacks(removeAllStatusEffect.StatusEffectType);
            target.RemoveStatusEffect(removeAllStatusEffect.StatusEffectType,stackCount);
        }

        yield return null;
    }

    private void PreStatusEffectReaction(AddStatusEffectGA addStatusEffectGA)
    {
        if (addStatusEffectGA.StatusEffectType == StatusEffectType.BLOCK)
        {
            int stackCount = addStatusEffectGA.StackCount;

            CombatantView caster = addStatusEffectGA.Caster;

            if (stackCount > 0 && caster != null)
            {
                stackCount += caster.GetStatusEffectStacks(StatusEffectType.DEXTERITY);

                stackCount = Mathf.Max(0, stackCount);

                if (caster.GetStatusEffectStacks(StatusEffectType.FRAIL) > 0)
                    stackCount = Mathf.FloorToInt(FRAIL_MULITPLIER * stackCount);

                addStatusEffectGA.SetStackCount(stackCount);
            }
        }
    }

    public void PrePostModifyStatusEffect(CombatantView combatantView, ReactionTiming reactionTiming)
    {
        List<StatusEffectType> activeStatusEffects = combatantView.GetAllActiveStatusEffects();

        foreach (StatusEffectType statusEffectType in activeStatusEffects)
        {
            StatusEffectModification modification = StatusEffectModification.NONE;

            switch (reactionTiming)
            {
                case ReactionTiming.PRE:
                    modification = _statusEffectData.Map[statusEffectType].PreTurnModification;
                    break;
                case ReactionTiming.POST:
                    modification = _statusEffectData.Map[statusEffectType].PostTurnModification;
                    break;
            }

            AddStatusEffectGA addStatusEffectGA;

            switch (modification)
            {
                case StatusEffectModification.NONE:
                    break;
                case StatusEffectModification.REMOVE_ALL:
                    int stack = combatantView.GetStatusEffectStacks(statusEffectType);
                    addStatusEffectGA = new(statusEffectType, -stack, new() { combatantView });
                    ActionSystem.Instance.AddReaction(addStatusEffectGA);
                    break;
                case StatusEffectModification.REMOVE_ONE:
                    addStatusEffectGA = new(statusEffectType, -1, new() { combatantView });
                    ActionSystem.Instance.AddReaction(addStatusEffectGA);
                    break;
                case StatusEffectModification.APPLY_EFFECT:
                    GameAction gameAction = _statusEffectData.Map[statusEffectType].Effect.GetGameAction(null, new() { combatantView });
                    ActionSystem.Instance.AddReaction(gameAction);
                    break;
            }
        }
    }

    public void PrePostModifyStatusEffect(List<CombatantView> combatantViews, ReactionTiming reactionTiming)
    {
        IEnumerable<StatusEffectType> activeStatusEffects = combatantViews.SelectMany(c => c.GetAllActiveStatusEffects()).Distinct();

        foreach (StatusEffectType statusEffectType in activeStatusEffects)
        {
            IEnumerable<CombatantView> relevantTargets = combatantViews.Where(c => c.GetStatusEffectStacks(statusEffectType) != 0);
            StatusEffectModification modification = StatusEffectModification.NONE;

            switch (reactionTiming)
            {
                case ReactionTiming.PRE:
                    modification = _statusEffectData.Map[statusEffectType].PreTurnModification;
                    break;
                case ReactionTiming.POST:
                    modification = _statusEffectData.Map[statusEffectType].PostTurnModification;
                    break;
            }

            AddStatusEffectGA addStatusEffectGA;

            switch (modification)
            {
                case StatusEffectModification.NONE:
                    break;
                case StatusEffectModification.REMOVE_ALL:
                    RemoveAllStatusEffectGA removeAllStatusEffect = new(statusEffectType, relevantTargets.ToList());
                    ActionSystem.Instance.AddReaction(removeAllStatusEffect);
                    break;
                case StatusEffectModification.REMOVE_ONE:
                    addStatusEffectGA = new(statusEffectType, -1, relevantTargets.ToList());
                    ActionSystem.Instance.AddReaction(addStatusEffectGA);
                    break;
                case StatusEffectModification.APPLY_EFFECT:
                    GameAction gameAction = _statusEffectData.Map[statusEffectType].Effect.GetGameAction(null, relevantTargets.ToList());
                    ActionSystem.Instance.AddReaction(gameAction);
                    break;
            }
        }
    }

    public static string StackAdditionValueFromEffect(StatusEffectType type, int baseStacks, CombatantView caster, List<CombatantView> targets = null)
    {
        int modifiedValue = ModifiedStackValue(type, baseStacks, caster, targets);

        if (baseStacks > modifiedValue)
        {
            return "<color=\"red\">" + modifiedValue.ToString() + "</color>";
        }
        else if (baseStacks < modifiedValue)
        {
            return "<color=\"green\">" + modifiedValue.ToString() + "</color>";
        }

        return baseStacks.ToString();
    }

    private static int ModifiedStackValue(StatusEffectType type, int baseStacks, CombatantView caster, List<CombatantView> targets = null)
    {
        if (type == StatusEffectType.BLOCK)
        {
            if (baseStacks > 0)
            {
                baseStacks = Mathf.Max(0, baseStacks + caster.GetStatusEffectStacks(StatusEffectType.DEXTERITY));

                float modifier = 1.0f;

                if (caster.GetStatusEffectStacks(StatusEffectType.FRAIL) > 0)
                    modifier *= FRAIL_MULITPLIER;

                return Mathf.CeilToInt(baseStacks * modifier);
            }
        }

        return baseStacks;
    }

    public StatusEffectInfo GetStatusEffectInfo(StatusEffectType statusEffectType)
    {
        return _statusEffectData.Map[statusEffectType];
    }
}
