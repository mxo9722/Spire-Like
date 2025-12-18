using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LaneView : MonoBehaviour, ITargetPreviewable
{
    [field: SerializeField] public SlotView HeroSlot { get; private set; }
    [field: SerializeField] public List<SlotView> EnemySlots { get; private set; }

    [SerializeField] private SpriteRenderer _targetPreviewSR;

    public List<EnemyView> EnemyViews { get => EnemySlots.Select(s => (EnemyView)s.Combatant).Where(c => c != null).ToList(); }
    public HeroView HeroView { get => (HeroView)HeroSlot.Combatant; }
    public bool Dead { get; private set; } = false;

    public bool TargetPreviewActive => _targetPreviewSR.color != Color.clear;

    public SlotView FirstAvailableEnemySlot()
    {
        foreach (SlotView slot in EnemySlots)
        {
            if (slot.IsEmpty)
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
        Coroutine wait = null;

        for(int i=0;i<EnemyViews.Count;i++)
        {
            EnemyView enemyView = EnemyViews[i];
            SlotView enemySlot = EnemySlots[i];

            if (enemyView.Slot != enemySlot)
            {
                yield return enemyView.WaitForTweensComplete();
                enemySlot.AddCombatant(enemyView, false);
                wait = StartCoroutine(enemySlot.PullCombatant(0.4f));
            }
        }

        if (wait != null && pauseForMove)
            yield return wait;
    }

    public void AddEnemy(EnemyView enemyView, LaneView originalLaneView = null)
    {
        SlotView slot = FirstAvailableEnemySlot();

        if (slot == null)
            return;

        slot.AddCombatant(enemyView);
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

        enemyView.Slot.RemoveCombatant();

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

        HeroSlot.AddCombatant(heroView);

        if (originalLaneView == null)
            heroView.transform.localPosition = Vector3.zero;
        else
        {
            //TODO: Add move hero functionality
        }
    }

    public IEnumerator SwapHero(HeroView heroView, float duration)
    {
        if (heroView != null)
        {
            HeroSlot.AddCombatant(heroView, false);
            yield return HeroSlot.PullCombatant(0.4f);
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
