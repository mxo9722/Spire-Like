using AYellowpaper.SerializedCollections;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySystem : Singleton<EnemySystem>
{
    [field: SerializeField] public EnemyActionData EnemyActionSymbolData { get; private set; }

    private BoardView _boardView;

    void OnEnable()
    {
        ActionSystem.AttachPerformer<NPCTurnGA>(NPCTurnPerformer);
        ActionSystem.AttachPerformer<NPCActGA>(NPCActPerformer);
        ActionSystem.AttachPerformer<NPCBehaviorTextGA>(NPCBehaviorTextPerformer);
        ActionSystem.AttachPerformer<DetermineNPCBehaviorGA>(DetermineNPCBehaviorPerformer);
        ActionSystem.AttachPerformer<HideEnemyPreviewGA>(HideEnemyPreviewPerformer);
        ActionSystem.AttachPerformer<AttackHeroGA>(AttackHeroPerformer);
        ActionSystem.AttachPerformer<MultiAttackHeroGA>(MultiAttackHeroPerformer);
        ActionSystem.AttachPerformer<KillNpcGA>(KillNpcPerformer);
    }

    void OnDisable()
    {
        ActionSystem.DetachPerformer<NPCTurnGA>();
        ActionSystem.DetachPerformer<NPCActGA>();
        ActionSystem.DetachPerformer<NPCBehaviorTextGA>();
        ActionSystem.DetachPerformer<DetermineNPCBehaviorGA>();
        ActionSystem.DetachPerformer<HideEnemyPreviewGA>();
        ActionSystem.DetachPerformer<AttackHeroGA>();
        ActionSystem.DetachPerformer<MultiAttackHeroGA>();
        ActionSystem.DetachPerformer<KillNpcGA>();
    }

    public void Setup(List<NPCData> laneData, int index)
    {
        _boardView = BoardSystem.Instance.BoardView;

        if (laneData == null)
            return;

        foreach (NPCData enemyData in laneData)
        {
            _boardView.CreateEnemy(enemyData, index);
        }
    }

    private IEnumerator NPCTurnPerformer(NPCTurnGA enemyTurnGA)
    {
        List<NPCView> allViews = enemyTurnGA.Targets;

        //List<CombatantView> burnEnemies = new(allEnemyViews);
        //burnEnemies = burnEnemies.FindAll(e => e.GetStatusEffectStacks(StatusEffectType.BURN) > 0);

        //ApplyBurnGA applyBurnGA = new(burnEnemies);
        //ActionSystem.Instance.AddReaction(applyBurnGA);

        foreach (NPCView npcView in allViews)
        {
            EffectContext context = new(npcView);

            NPCActGA npcActGA = new(context, npcView);
            ActionSystem.Instance.AddReaction(npcActGA);
        }

        CompressBoardGA compressBoardGA = new();
        //ActionSystem.Instance.AddReaction(compressBoardGA);

        foreach (NPCView npcView in allViews)
        {
            DetermineNPCBehaviorGA determineEnemyBehaviorGA = new(npcView);
            ActionSystem.Instance.AddReaction(determineEnemyBehaviorGA);
        }

        yield return null;
    }

    private IEnumerator NPCActPerformer(NPCActGA npcActGA)
    {
        NPCView npcView = npcActGA.NPC;

        if (npcView.CurrentHealth == 0)
            yield break;

        EffectContext context = new(npcView);

        //List<GameAction> gameActions = new();

        NPCBehaviorTextGA npcBehaviorTextGA = new(npcView, npcView.CurrentAction.Name);
        ActionSystem.Instance.AddReaction(npcBehaviorTextGA);

        foreach (AutoTargetEffect effect in npcView.CurrentAction.Effects)
        {
            AutoTargetEffectGA autoTargetEffectGA = new(context, effect);
            //gameActions.Add(autoTargetEffectGA);
            ActionSystem.Instance.AddReaction(autoTargetEffectGA);
        }

        HideEnemyPreviewGA hideEnemyPreviewGA = new(npcView);
        ActionSystem.Instance.AddReaction(hideEnemyPreviewGA);

        yield return null;
    }

    private IEnumerator NPCBehaviorTextPerformer(NPCBehaviorTextGA npcBehaviorTextGA)
    {
        yield return npcBehaviorTextGA.Target.ApplyBehaviorText(npcBehaviorTextGA.BehaviorName, 0.75f);
    }

    private IEnumerator DetermineNPCBehaviorPerformer(DetermineNPCBehaviorGA determineEnemyBehaviorGA)
    {
        if (determineEnemyBehaviorGA.EnemyView.CurrentHealth == 0)
            yield break;

        DetermineNPCBehaviour(determineEnemyBehaviorGA.EnemyView);

        yield return null;
    }

    private IEnumerator HideEnemyPreviewPerformer(HideEnemyPreviewGA hideEnemyPreviewGA)
    {
        if (hideEnemyPreviewGA.EnemyView.CurrentHealth > 0)
            hideEnemyPreviewGA.EnemyView.SetCurrentAction(null);
        yield return null;
    }

    private IEnumerator AttackHeroPerformer(AttackHeroGA attackHeroGA)
    {
        if (attackHeroGA.Caster.CurrentHealth == 0 || attackHeroGA.Targets.Count == 0)
        {
            attackHeroGA.Context.AddData(attackHeroGA.OnHitKey, false);
            yield return null;
        }
        else
        {
            CombatantView attacker = attackHeroGA.Caster;
            attackHeroGA.Context.AddData(attackHeroGA.OnHitKey, true);

            yield return attacker.WaitForTweensComplete();

            Tween tween = attacker.transform.DOMoveX(attacker.transform.position.x - 1.0f, 0.15f);
            yield return tween.WaitForCompletion();
            tween = attacker.transform.DOMoveX(attacker.transform.position.x + 1.0f, 0.25f);

            DealDamageGA dealDamageGA = new(attackHeroGA.Damage, attackHeroGA.Targets, attackHeroGA.Context);
            dealDamageGA.SetUnblockedKey(attackHeroGA.UnblockedKey);
            dealDamageGA.SetOverkillKey(attackHeroGA.OverkillKey);

            ActionSystem.Instance.AddReaction(dealDamageGA);
            yield return tween.WaitForCompletion();
        }
    }

    private IEnumerator MultiAttackHeroPerformer(MultiAttackHeroGA arg)
    {
        for (int i = 0; i < arg.AttackTimes; i++)
        {
            AttackHeroGA attackHeroGA = new(arg.Damage, arg.Targets, arg.Context, arg.UnblockedKey, arg.OverkillKey, arg.OnHitKey);
            ActionSystem.Instance.AddReaction(attackHeroGA);
        }
        yield return 0;
    }

    private IEnumerator KillNpcPerformer(KillNpcGA killEnemyGA)
    {
        Coroutine coroutine = null;

        foreach (NPCView npc in killEnemyGA.NPCViews)
        {
            npc.SetHealth(0);
            coroutine = StartCoroutine(_boardView.RemoveEnemy(npc));
        }

        if (coroutine != null)
            yield return coroutine;

        if (BoardSystem.Instance.GetAllEnemies().Count == 0)
        {
            MatchEndSystem.Instance.EndCombat();
        }
    }

    public Sprite GetEnemyActionSymbol(NPCActionType enemyActionSymbolType)
    {
        Sprite sprite = EnemyActionSymbolData.EnemyActionTypes[enemyActionSymbolType].Sprite;

        return sprite;
    }

    public Sprite GetEnemyTargetSymbol(NPCTargetTypes enemyActionSymbolType)
    {
        return EnemyActionSymbolData.EnemyTargetTypes[enemyActionSymbolType].Sprite;
    }

    public string GetEnemyActionDescription(NPCActionType enemyActionSymbolType)
    {
        return EnemyActionSymbolData.EnemyActionTypes[enemyActionSymbolType].Text;
    }

    public string GetEnemyTargetDescription(NPCTargetTypes enemyActionSymbolType)
    {
        return EnemyActionSymbolData.EnemyTargetTypes[enemyActionSymbolType].Text;
    }

    public void UpdateEnemiesBehaviorUI()
    {
        List<NPCView> enemies = BoardSystem.Instance.BoardView.GetAllEnemies();
        foreach (NPCView enemyView in enemies)
        {
            enemyView.UpdateBehaviorIndicator();
        }
    }

    public static void DetermineNPCBehaviour(NPCView npcView)
    {
        List<NPCAction> fullPattern = npcView.Data.ActionPattern;

        List<NPCAction> validPattern = new();
        float validWeight = 0;
        int highPriority = 0;

        foreach (NPCAction enemyAction in fullPattern)
        {
            int priority = enemyAction.Priority;

            if (IsEnemyActionValid(enemyAction, npcView) && priority >= highPriority)
            {
                if (highPriority < priority)
                {
                    validPattern.Clear();
                    validWeight = 0;
                    highPriority = priority;
                }

                validPattern.Add(enemyAction);
                validWeight += enemyAction.Weight;
            }
        }

        float randomValue = (float)(RNG.Random.NextDouble() * validWeight);

        foreach (NPCAction enemyAction in validPattern)
        {
            if (enemyAction.Weight < randomValue)
            {
                randomValue -= enemyAction.Weight;
            }
            else
            {
                npcView.SetCurrentAction(enemyAction);
                break;
            }
        }
    }

    private static bool IsEnemyActionValid(NPCAction enemyAction, NPCView enemyView)
    {
        EffectContext conditionContext = new(enemyView);

        foreach (Condition condition in enemyAction.Conditions)
        {
            if (!condition.TestCondition(conditionContext))
                return false;
        }

        if (enemyAction.ConsecutiveMax != 0 && enemyAction.ConsecutiveMax <= enemyView.PreviousActions.Count)
        {
            List<NPCAction> previousActionsSubset = enemyView.PreviousActions.GetRange(enemyView.PreviousActions.Count - enemyAction.ConsecutiveMax, enemyAction.ConsecutiveMax);

            if (!previousActionsSubset.Any(e => e != enemyAction))
                return false;
        }

        return true;
    }
}
