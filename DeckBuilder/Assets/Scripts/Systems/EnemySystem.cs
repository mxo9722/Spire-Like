using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySystem : Singleton<EnemySystem>
{
    [SerializeField] private EnemyBoardView _enemyBoardView;
    public List<EnemyView> Enemies => _enemyBoardView.EnemyViews;
    void OnEnable()
    {
        ActionSystem.AttachPerformer<EnemyTurnGA>(EnemyTurnPerformer);
        ActionSystem.AttachPerformer<AttackHeroGA>(AttackHeroPerformer);
        ActionSystem.AttachPerformer<KillEnemyGA>(KillEnemyPerformer);
    }

    void OnDisable()
    {
        ActionSystem.DetachPerformer<EnemyTurnGA>();
        ActionSystem.DetachPerformer<AttackHeroGA>();
        ActionSystem.DetachPerformer<KillEnemyGA>();
    }

    public void Setup(List<EnemyData> enemyDatas)
    {
        foreach(EnemyData enemyData in enemyDatas)
        {
            _enemyBoardView.AddEnemy(enemyData);
        }
    }

    private IEnumerator EnemyTurnPerformer(EnemyTurnGA enemyTurnGA)
    {
        foreach (EnemyView enemy in _enemyBoardView.EnemyViews)
        {
            int burnStack = enemy.GetStatusEffectStacks(StatusEffectType.BURN);

            if(burnStack > 0)
            {
                ApplyBurnGA applyBurnGA = new(burnStack, enemy);
                ActionSystem.Instance.AddReaction(applyBurnGA);
            }

            AttackHeroGA attackHeroGA = new(enemy.AttackPower, enemy);
            ActionSystem.Instance.AddReaction(attackHeroGA);
        }

        yield return null;
    }

    private IEnumerator AttackHeroPerformer(AttackHeroGA attackHeroGA)
    {
        if (attackHeroGA.Attacker.CurrentHealth == 0)
            yield return null;
        else
        {
            EnemyView attacker = attackHeroGA.Attacker;
            Tween tween = attacker.transform.DOMoveX(attacker.transform.position.x - 1.0f, 0.15f);
            yield return tween.WaitForCompletion();
            tween = attacker.transform.DOMoveX(attacker.transform.position.x + 1.0f, 0.25f);
            DealDamageGA dealDamageGA = new(attacker.AttackPower, HeroSystem.Instance.HeroView, attacker);
            ActionSystem.Instance.AddReaction(dealDamageGA);
            yield return tween.WaitForCompletion();
        }
    }

    private IEnumerator KillEnemyPerformer(KillEnemyGA killEnemyGA)
    {
        yield return _enemyBoardView.RemoveEnemy(killEnemyGA.EnemyView);
    }
}
