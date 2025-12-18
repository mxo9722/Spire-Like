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
        ActionSystem.AttachPerformer<EnemyTurnGA>(EnemyTurnPerformer);
        ActionSystem.AttachPerformer<DetermineEnemyBehaviorGA>(DetermineEnemyBehaviorPerformer);
        ActionSystem.AttachPerformer<HideEnemyPreviewGA>(HideEnemyPreviewPerformer);
        ActionSystem.AttachPerformer<AttackHeroGA>(AttackHeroPerformer);
        ActionSystem.AttachPerformer<MultiAttackHeroGA>(MultiAttackHeroPerformer);
        ActionSystem.AttachPerformer<KillEnemyGA>(KillEnemyPerformer);
    }

    void OnDisable()
    {
        ActionSystem.DetachPerformer<EnemyTurnGA>();
        ActionSystem.DetachPerformer<DetermineEnemyBehaviorGA>();
        ActionSystem.DetachPerformer<HideEnemyPreviewGA>();
        ActionSystem.DetachPerformer<AttackHeroGA>();
        ActionSystem.DetachPerformer<MultiAttackHeroGA>();
        ActionSystem.DetachPerformer<KillEnemyGA>();
    }

    public void Setup(List<EnemyData> laneData, int index)
    {
        _boardView = BoardSystem.Instance.BoardView;

        if (laneData == null)
            return;

        foreach (EnemyData enemyData in laneData)
        {
            _boardView.CreateEnemy(enemyData, index);
        }
    }

    private IEnumerator EnemyTurnPerformer(EnemyTurnGA enemyTurnGA)
    {
        List<EnemyView> allEnemyViews = _boardView.GetAllEnemies();

        //List<CombatantView> burnEnemies = new(allEnemyViews);
        //burnEnemies = burnEnemies.FindAll(e => e.GetStatusEffectStacks(StatusEffectType.BURN) > 0);

        //ApplyBurnGA applyBurnGA = new(burnEnemies);
        //ActionSystem.Instance.AddReaction(applyBurnGA);

        allEnemyViews = _boardView.GetAllEnemies();

        foreach (EnemyView enemyView in allEnemyViews)
        {
            EffectContext targetModeContext = EffectContext.CreateEnemyEC(enemyView);

            HideEnemyPreviewGA hideEnemyPreviewGA = new(enemyView);
            ActionSystem.Instance.AddReaction(hideEnemyPreviewGA);

            foreach (AutoCombatantTargetEffect effect in enemyView.CurrentAction.Effects)
            {
                GameAction gameAction = effect.Effect.GetGameAction(new(enemyView), combatantTargets:effect.TargetMode.GetTargets(targetModeContext));
                ActionSystem.Instance.AddReaction(gameAction);
            }
        }

        CompressBoardGA compressBoardGA = new();
        ActionSystem.Instance.AddReaction(compressBoardGA);

        foreach (EnemyView enemyView in allEnemyViews)
        {
            DetermineEnemyBehaviorGA determineEnemyBehaviorGA = new(enemyView);
            ActionSystem.Instance.AddReaction(determineEnemyBehaviorGA);
        }

        yield return null;
    }

    private IEnumerator DetermineEnemyBehaviorPerformer(DetermineEnemyBehaviorGA determineEnemyBehaviorGA)
    {
        if (determineEnemyBehaviorGA.EnemyView.CurrentHealth == 0)
            yield break;

        DetermineEnemyBehaviour(determineEnemyBehaviorGA.EnemyView);

        yield return null;
    }

    private IEnumerator HideEnemyPreviewPerformer(HideEnemyPreviewGA hideEnemyPreviewGA)
    {
        if(hideEnemyPreviewGA.EnemyView.CurrentHealth > 0)
            hideEnemyPreviewGA.EnemyView.SetCurrentAction(null);
        yield return null;
    }

    private IEnumerator AttackHeroPerformer(AttackHeroGA attackHeroGA)
    {
        if (attackHeroGA.Attacker.CurrentHealth == 0 || attackHeroGA.Targets.Count == 0)
            yield return null;
        else
        {
            EnemyView attacker = attackHeroGA.Attacker;

            yield return attacker.WaitForTweensComplete();

            Tween tween = attacker.transform.DOMoveX(attacker.transform.position.x - 1.0f, 0.15f);
            yield return tween.WaitForCompletion();
            tween = attacker.transform.DOMoveX(attacker.transform.position.x + 1.0f, 0.25f);

            DealDamageGA dealDamageGA = new(attackHeroGA.Damage, attackHeroGA.Targets, attacker);
            
            ActionSystem.Instance.AddReaction(dealDamageGA);
            yield return tween.WaitForCompletion();
        }
    }

    private IEnumerator MultiAttackHeroPerformer(MultiAttackHeroGA arg)
    {
        for(int i = 0; i < arg.AttackTimes; i++)
        {
            AttackHeroGA attackHeroGA = new(arg.Damage,arg.Targets,arg.Attacker);
            ActionSystem.Instance.AddReaction(attackHeroGA);
        }
        yield return 0;
    }

    private IEnumerator KillEnemyPerformer(KillEnemyGA killEnemyGA)
    {
        yield return _boardView.RemoveEnemy(killEnemyGA.EnemyView);

        if(BoardSystem.Instance.GetAllEnemies().Count == 0)
        {
            MatchEndSystem.Instance.EndCombat();
        }
    }

    public Sprite GetEnemyActionSymbol(EnemyActionType enemyActionSymbolType)
    {
        return EnemyActionSymbolData.EnemyActionTypes[enemyActionSymbolType].Sprite;
    }

    public Sprite GetEnemyTargetSymbol(EnemyTargetTypes enemyActionSymbolType)
    {
        return EnemyActionSymbolData.EnemyTargetTypes[enemyActionSymbolType].Sprite;
    }
    
    public string GetEnemyActionDescription(EnemyActionType enemyActionSymbolType)
    {
        return EnemyActionSymbolData.EnemyActionTypes[enemyActionSymbolType].Text;
    }

    public string GetEnemyTargetDescription(EnemyTargetTypes enemyActionSymbolType)
    {
        return EnemyActionSymbolData.EnemyTargetTypes[enemyActionSymbolType].Text;
    }

    public void UpdateEnemiesBehaviorUI()
    {
        List<EnemyView> enemies = BoardSystem.Instance.BoardView.GetAllEnemies();
        foreach(EnemyView enemyView in enemies)
        {
            enemyView.UpdateBehaviorIndicator();
        }
    }

    public static void DetermineEnemyBehaviour(EnemyView enemyView)
    {
        List<EnemyAction> fullPattern = enemyView.Data.ActionPattern;

        List<EnemyAction> validPattern = new();
        float validWeight = 0;
        int highPriority = 0;

        foreach (EnemyAction enemyAction in fullPattern)
        {
            int priority = enemyAction.Priority;

            if (IsEnemyActionValid(enemyAction, enemyView) && priority >= highPriority)
            {
                if(highPriority < priority)
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

        foreach (EnemyAction enemyAction in validPattern)
        {
            if (enemyAction.Weight < randomValue)
            {
                randomValue -= enemyAction.Weight;
            }
            else
            {
                enemyView.SetCurrentAction(enemyAction);
                break;
            }
        }
    }

    private static bool IsEnemyActionValid(EnemyAction enemyAction, EnemyView enemyView)
    {
        EffectContext conditionContext = new(enemyView);

        foreach(Condition condition in enemyAction.Conditions)
        {
            if (!condition.TestCondition(conditionContext))
                return false;
        }

        if(enemyAction.ConsecutiveMax != 0 && enemyAction.ConsecutiveMax <= enemyView.PreviousActions.Count)
        {
            List<EnemyAction> previousActionsSubset = enemyView.PreviousActions.GetRange(enemyView.PreviousActions.Count - enemyAction.ConsecutiveMax, enemyAction.ConsecutiveMax);

            if (!previousActionsSubset.Any(e => e != enemyAction))
                return false;
        }

        return true;
    }
}
