using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StatusEffectSystem : Singleton<StatusEffectSystem>
{

    [SerializeField] private ParticleSystem _defendVFX;
    [SerializeField] private StatusEffectsDictionary _statusEffectData;

    public const float FRAIL_MULITPLIER = 0.75f;

    private List<SEReactionHandler> _statusEffectHandlers = new();

    private void OnEnable()
    {
        ActionSystem.AttachPerformer<AddStatusEffectGA>(AddStatusEffectPerformer);
        ActionSystem.AttachPerformer<ConvertStatusEffectGA>(ConvertStatusEffectPerformer);
        ActionSystem.AttachPerformer<MultiplyStatusEffectGA>(MultiplyStatusEffectPerformer);
        ActionSystem.AttachPerformer<RemoveAllStatusEffectGA>(RemoveAllStatusEffectPerformer);
        ActionSystem.AttachPerformer<SetStatusEffectGA>(SetStatusEffectPerformer);
        ActionSystem.AttachPerformer<TransferSE_GA>(TransferSEPerformer);

        ActionSystem.SubscribeReaction<NPCTurnGA>(this, PreNPCTurnReaction, ReactionTiming.PRE);
        ActionSystem.SubscribeReaction<NPCTurnGA>(this, PostNPCTurnReaction, ReactionTiming.POST);
    }

    private void OnDisable()
    {
        ActionSystem.DetachPerformer<AddStatusEffectGA>();
        ActionSystem.DetachPerformer<ConvertStatusEffectGA>();
        ActionSystem.DetachPerformer<MultiplyStatusEffectGA>();
        ActionSystem.DetachPerformer<RemoveAllStatusEffectGA>();
        ActionSystem.DetachPerformer<SetStatusEffectGA>();
        ActionSystem.DetachPerformer<TransferSE_GA>();

        ActionSystem.UnsubscribeReaction<NPCTurnGA>(this, PreNPCTurnReaction, ReactionTiming.PRE);
        ActionSystem.UnsubscribeReaction<NPCTurnGA>(this, PostNPCTurnReaction, ReactionTiming.POST);
    }

    private IEnumerator AddStatusEffectPerformer(AddStatusEffectGA addStatusEffectGA)
    {
        float waitTime = 0;
        int stackCount = ModifiedStackValue(addStatusEffectGA.StatusEffectInfo, addStatusEffectGA.StackCount, addStatusEffectGA.Context);

        if (!addStatusEffectGA.SkipAnimation)
            foreach (CombatantView target in addStatusEffectGA.Targets)
            {
                if (target.CurrentHealth <= 0)
                    continue;
                switch (addStatusEffectGA.StatusEffectInfo)
                {
                    default:
                        if (stackCount > 0)
                        {
                            ParticleSystem effect = Instantiate(_defendVFX, target.transform);

                            effect.textureSheetAnimation.SetSprite(0, addStatusEffectGA.StatusEffectInfo.Sprite);

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
            target.AddStatusEffect(addStatusEffectGA.StatusEffectInfo, stackCount);
            ///TODO: Add a special effect

            yield return null;
        }

        DynamicViewsSystem.Instance.UpdateDynamicValues();
    }

    private IEnumerator ConvertStatusEffectPerformer(ConvertStatusEffectGA convertStatusEffectGA)
    {
        foreach (CombatantView target in convertStatusEffectGA.Targets)
        {
            int stackCount = target.GetStatusEffectStacks(convertStatusEffectGA.From);

            if (convertStatusEffectGA.UpTo >= 0)
                stackCount = Mathf.Min(convertStatusEffectGA.UpTo, stackCount);

            AddStatusEffectGA removeStatusEffect = new(convertStatusEffectGA.From, -stackCount, convertStatusEffectGA.Targets, convertStatusEffectGA.Context);
            ActionSystem.Instance.AddReaction(removeStatusEffect);

            AddStatusEffectGA addStatusEffect = new(convertStatusEffectGA.To, stackCount, convertStatusEffectGA.Targets, convertStatusEffectGA.Context);
            ActionSystem.Instance.AddReaction(addStatusEffect);
        }

        yield return null;
    }

    private IEnumerator MultiplyStatusEffectPerformer(MultiplyStatusEffectGA multiplyStatusEffectGA)
    {
        foreach (CombatantView unit in multiplyStatusEffectGA.Targets)
        {
            if (unit == null || unit.CurrentHealth == 0)
                continue;

            int amount = unit.GetStatusEffectStacks(multiplyStatusEffectGA.StatusEffect);
            if (multiplyStatusEffectGA.Multiplier > 1)
                amount = Mathf.CeilToInt(amount * multiplyStatusEffectGA.Multiplier - amount);
            else
                amount = Mathf.FloorToInt(amount * multiplyStatusEffectGA.Multiplier - amount);


            if (amount == 0)
                continue;

            AddStatusEffectGA addStatusEffect = new(multiplyStatusEffectGA.StatusEffect, amount, new() { unit }, new());
            ActionSystem.Instance.AddReaction(addStatusEffect);
        }

        yield return null;
    }

    private IEnumerator RemoveAllStatusEffectPerformer(RemoveAllStatusEffectGA removeAllStatusEffect)
    {
        foreach (CombatantView target in removeAllStatusEffect.Targets)
        {
            int stackCount = target.GetStatusEffectStacks(removeAllStatusEffect.StatusEffectInfo);
            target.RemoveStatusEffect(removeAllStatusEffect.StatusEffectInfo, stackCount);
        }

        yield return null;
    }

    private IEnumerator SetStatusEffectPerformer(SetStatusEffectGA setStatusEffectGA)
    {
        foreach (CombatantView target in setStatusEffectGA.Targets)
        {
            AddStatusEffectGA addStatusEffectGA = new(setStatusEffectGA.StatusEffectInfo, 
                setStatusEffectGA.StackCount - target.GetStatusEffectStacks(setStatusEffectGA.StatusEffectInfo), 
                new() { target }, setStatusEffectGA.Context, setStatusEffectGA.SkipAnimation);

            ActionSystem.Instance.AddReaction(addStatusEffectGA);
        }


        yield return null;
    }


    private IEnumerator TransferSEPerformer(TransferSE_GA transferSE_GA)
    {
        int amount = Math.Min(transferSE_GA.From.GetStatusEffectStacks(transferSE_GA.SEType), transferSE_GA.MaxTransferAmount);
        amount = Math.Max(0, amount);
        AddStatusEffectGA removeStatusEffect = new(transferSE_GA.SEType, -amount, new() { transferSE_GA.From }, new());
        ActionSystem.Instance.AddReaction(removeStatusEffect);
        AddStatusEffectGA addStatusEffect = new(transferSE_GA.SEType, amount, transferSE_GA.To, new());
        ActionSystem.Instance.AddReaction(addStatusEffect);

        yield return null;
    }

    private void PreNPCTurnReaction(NPCTurnGA npcTurnGA)
    {
        PrePostModifyStatusEffect(npcTurnGA.Targets.Cast<CombatantView>().ToList(), ReactionTiming.PRE);
    }

    private void PostNPCTurnReaction(NPCTurnGA npcTurnGA)
    {
        PrePostModifyStatusEffect(npcTurnGA.Targets.Cast<CombatantView>().ToList(), ReactionTiming.POST);
    }

    public void PrePostModifyStatusEffect(CombatantView combatantView, ReactionTiming reactionTiming)
    {
        List<StatusEffectInfo> activeStatusEffects = combatantView.GetAllActiveStatusEffects();

        foreach (StatusEffectInfo statusEffectInfo in activeStatusEffects)
        {
            StatusEffectModification modification = StatusEffectModification.NONE;

            switch (reactionTiming)
            {
                case ReactionTiming.PRE:
                    modification = statusEffectInfo.PreTurnModification;
                    break;
                case ReactionTiming.POST:
                    modification = statusEffectInfo.PostTurnModification;
                    break;
            }

            AddStatusEffectGA addStatusEffectGA;

            switch (modification)
            {
                case StatusEffectModification.NONE:
                    break;
                case StatusEffectModification.REMOVE_ALL:
                    int stack = combatantView.GetStatusEffectStacks(statusEffectInfo);


                    addStatusEffectGA = new(statusEffectInfo, -stack, new() { combatantView }, new());

                    ActionSystem.Instance.AddReaction(addStatusEffectGA);
                    break;
                case StatusEffectModification.REMOVE_ONE:
                    addStatusEffectGA = new(statusEffectInfo, -1, new() { combatantView }, new());
                    ActionSystem.Instance.AddReaction(addStatusEffectGA);
                    break;
                case StatusEffectModification.APPLY_EFFECT:
                    GameAction gameAction = statusEffectInfo.Effect.GetGameAction(new(), new() { combatantView });
                    ActionSystem.Instance.AddReaction(gameAction);
                    break;
            }
        }
    }

    public void PrePostModifyStatusEffect(List<CombatantView> combatantViews, ReactionTiming reactionTiming)
    {
        IEnumerable<StatusEffectInfo> activeStatusEffects = combatantViews.SelectMany(c => c.GetAllActiveStatusEffects()).Distinct();

        foreach (StatusEffectInfo statusEffectTypeInfo in activeStatusEffects)
        {

            IEnumerable<CombatantView> relevantTargets = combatantViews.Where(c => c.GetStatusEffectStacks(statusEffectTypeInfo) != 0);
            StatusEffectModification modification = StatusEffectModification.NONE;

            switch (reactionTiming)
            {
                case ReactionTiming.PRE:
                    modification = statusEffectTypeInfo.PreTurnModification;
                    break;
                case ReactionTiming.POST:
                    modification = statusEffectTypeInfo.PostTurnModification;
                    break;
            }

            AddStatusEffectGA addStatusEffectGA;

            switch (modification)
            {
                case StatusEffectModification.NONE:
                    break;
                case StatusEffectModification.REMOVE_ALL:
                    RemoveAllStatusEffectGA removeAllStatusEffect = new(statusEffectTypeInfo, relevantTargets.ToList());
                    ActionSystem.Instance.AddReaction(removeAllStatusEffect);
                    break;
                case StatusEffectModification.REMOVE_ONE:
                    addStatusEffectGA = new(statusEffectTypeInfo, -1, relevantTargets.ToList(), new());
                    ActionSystem.Instance.AddReaction(addStatusEffectGA);
                    break;
                case StatusEffectModification.APPLY_EFFECT:
                    GameAction gameAction = statusEffectTypeInfo.Effect.GetGameAction(null, relevantTargets.ToList());
                    ActionSystem.Instance.AddReaction(gameAction);
                    break;
            }
        }
    }

    public void TrySubscribeSEReactions(StatusEffectInfo info)
    {
        if (_statusEffectHandlers.Any(handler => handler.Info == info))
            return;

        SEReactionHandler handler = new(info);

        _statusEffectHandlers.Add(handler);
        
        foreach(StatusEffectReaction reaction in info.Reactions)
        {
            reaction.SubscribeCondition(handler, handler.HandleReaction);
        }
    }
    
    public void TryUnsubscribeSEReactions(StatusEffectInfo info, CombatantView owner)
    {
        if (BoardSystem.Instance.GetAllCombatants().Any(c => c.HasStatusEffectUI(info) && c != owner))
            return;

        SEReactionHandler handler = _statusEffectHandlers.FirstOrDefault(seh => seh.Info == info);

        _statusEffectHandlers.Remove(handler);

        foreach (StatusEffectReaction reaction in info.Reactions)
        {
            reaction.UnsubscribeCondition(handler, handler.HandleReaction);
        }
    }

    public static string StackAdditionValueFromEffect(StatusEffectInfo info, int baseStacks, EffectContext context, List<CombatantView> targets = null)
    {
        int modifiedValue = ModifiedStackValue(info, baseStacks, context, targets);

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

    private static int ModifiedStackValue(StatusEffectInfo info, int baseStacks, EffectContext context, List<CombatantView> targets = null)
    {
        if (context.Caster == null && context.PlayedCard != null)
            context.SetCaster(context.PlayedCard.GetOwnerView(context));
        if (context.Caster == null)
            return baseStacks;

        if (info.EnumKey == StatusEffect.BLOCK)
        {
            if (baseStacks > 0)
            {
                if (context.Caster != null && context.PlayedCard != null)
                    baseStacks += context.Caster.GetStatusEffectStacks(StatusEffect.DEXTERITY);

                if (context.PlayedCard != null)
                {
                    ExtraBlockCM blockBonus = context.PlayedCard.GetCardModifier<ExtraBlockCM>();

                    if (blockBonus != null)
                        baseStacks += blockBonus.Amount;
                }

                baseStacks = Mathf.Max(0, baseStacks);

                float modifier = 1.0f;

                if (context.Caster.GetStatusEffectStacks(StatusEffect.FRAIL) > 0)
                    modifier *= FRAIL_MULITPLIER;

                return Mathf.FloorToInt(baseStacks * modifier);
            }
        }

        return baseStacks;
    }

    public static StatusEffectInfo GetDictionaryEntry(StatusEffect statusEffectType)
    {
        return Instance._statusEffectData.NewMap[statusEffectType].Info;
    }
}
