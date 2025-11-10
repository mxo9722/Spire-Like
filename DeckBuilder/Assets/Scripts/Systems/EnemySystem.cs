using AYellowpaper.SerializedCollections;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySystem : Singleton<EnemySystem>
{
    [field: SerializeField] public EnemyActionSymbolData EnemyActionSymbolData { get; private set; }

    private BoardView _boardView;

    void OnEnable()
    {
        ActionSystem.AttachPerformer<EnemyTurnGA>(EnemyTurnPerformer);
        ActionSystem.AttachPerformer<DetermineEnemyBehaviorGA>(DetermineEnemyBehaviorPerformer);
        ActionSystem.AttachPerformer<AttackHeroGA>(AttackHeroPerformer);
        ActionSystem.AttachPerformer<MultiAttackHeroGA>(MultiAttackHeroPerformer);
        ActionSystem.AttachPerformer<KillEnemyGA>(KillEnemyPerformer);
    }

    void OnDisable()
    {
        ActionSystem.DetachPerformer<EnemyTurnGA>();
        ActionSystem.DetachPerformer<AttackHeroGA>();
        ActionSystem.DetachPerformer<MultiAttackHeroGA>();
        ActionSystem.DetachPerformer<KillEnemyGA>();
    }

    public void Setup(List<EnemyData> laneData, int index)
    {
        _boardView = BoardSystem.Instance.BoardView;

        foreach (EnemyData enemyData in laneData)
        {
            _boardView.CreateEnemy(enemyData, index);
        }
    }

    private IEnumerator EnemyTurnPerformer(EnemyTurnGA enemyTurnGA)
    {
        List<EnemyView> allEnemyViews = _boardView.GetAllEnemies();

        foreach (EnemyView enemyView in allEnemyViews)
        {
            int burnStack = enemyView.GetStatusEffectStacks(StatusEffectType.BURN);

            if (burnStack > 0)
            {
                ApplyBurnGA applyBurnGA = new(burnStack, enemyView);
                ActionSystem.Instance.AddReaction(applyBurnGA);
            }

            TargetModeContext targetModeContext = new(enemyView);

            foreach (AutoTargetEffect effect in enemyView.CurrentAction.Effects)
            {
                GameAction gameAction = effect.Effect.GetGameAction(effect.TargetMode.GetTargets(targetModeContext), enemyView);

                ActionSystem.Instance.AddReaction(gameAction);
            }
        }

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

    private IEnumerator AttackHeroPerformer(AttackHeroGA attackHeroGA)
    {
        if (attackHeroGA.Attacker.CurrentHealth == 0 || attackHeroGA.Targets.Count == 0)
            yield return null;
        else
        {
            EnemyView attacker = attackHeroGA.Attacker;
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
    }

    public Sprite GetEnemyActionSymbol(EnemyActionSymbolType enemyActionSymbolType)
    {
        return EnemyActionSymbolData.Map[enemyActionSymbolType];
    }

    public static void DetermineEnemyBehaviour(EnemyView enemyView)
    {
        List<EnemyAction> fullPattern = enemyView.Data.ActionPattern;

        List<EnemyAction> validPattern = new();
        float validWeight = 0;

        foreach (EnemyAction enemyAction in fullPattern)
        {
            if (IsEnemyActionValid(enemyAction, enemyView))
            {
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
        ConditionContext conditionContext = new(enemyView);

        foreach(Condition condition in enemyAction.Conditions)
        {
            if (!condition.IsConditionMet(conditionContext))
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
