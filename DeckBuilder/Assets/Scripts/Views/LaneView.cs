using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LaneView : MonoBehaviour, ITargetPreviewable
{
    [field: SerializeField] public Transform HeroSlot { get; private set; }
    [field: SerializeField] public List<Transform> EnemySlots { get; private set; }

    [SerializeField] private SpriteRenderer _targetPreviewSR;

    public List<EnemyView> EnemyViews { get; private set; } = new();
    public HeroView HeroView { get; private set; } = null;
    public bool Dead { get; private set; } = false;

    public bool TargetPreviewActive => _targetPreviewSR.color != Color.clear;

    public Transform FirstAvailableSlot()
    {
        foreach (Transform slot in EnemySlots)
        {
            if (slot.childCount == 0)
                return slot;
        }

        return null;
    }

    public void Start()
    {
        _targetPreviewSR.transform.DORotate(new(0, 0, 180.0f), 2f, RotateMode.Fast).SetLoops(-1).SetEase(Ease.Linear);
        HideTargetPreview();
    }

    public IEnumerator SlideEnemiesLeft(float duration, bool pauseForMove = false)
    {
        Tween tween = null;

        for(int i=0;i<EnemyViews.Count;i++)
        {
            EnemyView enemyView = EnemyViews[i];
            Transform enemySlot = EnemySlots[i];

            if (enemyView.transform.parent != enemySlot)
            {
                enemyView.transform.parent = enemySlot;

                List<Tween> tweens = DOTween.TweensByTarget(enemyView.transform);

                if(tweens != null)
                {
                    foreach (Tween t in tweens)
                        yield return t;
                }

                tween = enemyView.transform.DOLocalMove(Vector3.zero, duration);
            }
        }

        if (tween != null && pauseForMove)
            yield return tween.WaitForCompletion();
    }

    public void AddEnemy(EnemyView enemyView, LaneView originalLaneView = null)
    {
        Transform slot = FirstAvailableSlot();

        if (slot == null)
            return;

        enemyView.transform.parent = slot;
        EnemyViews.Add(enemyView);

        if (originalLaneView != null)
        {
            MoveEnemyGA moveEnemyGA = new(this, enemyView);
            ActionSystem.Instance.AddReaction(moveEnemyGA);
        }
        else
        {
            enemyView.transform.localPosition = Vector3.zero;
        }
    }

    public IEnumerator RemoveEnemy(EnemyView enemyView)
    {
        if (!EnemyViews.Contains(enemyView))
            yield break;

        yield return enemyView.WaitForTweensComplete();

        EnemyViews.Remove(enemyView);

        Tween tween = enemyView.transform.DOScale(Vector3.zero, 0.25f);
        yield return tween.WaitForCompletion();
        Destroy(enemyView.gameObject);
        yield return SlideEnemiesLeft(0.2f);
    }

    public void SetHero(HeroView heroView, LaneView originalLaneView = null)
    {
        if (HeroView != null)
        {
            //TODO: Add swap hero functionality
            return;
        }

        heroView.transform.parent = HeroSlot;
        HeroView = heroView;

        if (originalLaneView == null)
            heroView.transform.localPosition = Vector3.zero;
        else
        {
            //TODO: Add move hero functionality
        }
    }

    public IEnumerator SwapHero(HeroView heroView, float duration)
    {
        HeroView = heroView;

        if (heroView != null)
        {
            yield return heroView.WaitForTweensComplete();

            heroView.transform.parent = HeroSlot;
            yield return heroView.transform.DOLocalJump(Vector3.zero, 2, 1, duration).WaitForCompletion();
        }
    }

    public bool Contains(CombatantView combatant)
    {
        return HeroView == combatant || EnemyViews.Any(e => e == combatant);
    }

    public bool IsValid(EffectContext context, List<LaneFilter> filters)
    {
        return !filters.Any(f => !f.TestTarget(context, this));
    }

    public void SetTargetPreview(Color color)
    {
        _targetPreviewSR.color = color;
    }

    public void HideTargetPreview()
    {
        _targetPreviewSR.color = Color.clear;
    }
}
