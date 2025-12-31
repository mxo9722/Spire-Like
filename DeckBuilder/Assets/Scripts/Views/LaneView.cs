using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LaneView : MonoBehaviour, ITargetPreviewable
{
    [field: SerializeField] public SlotView[] HeroSlots { get; private set; }
    [field: SerializeField] public SlotView[] EnemySlots { get; private set; }

    [SerializeField] private SpriteRenderer _targetPreviewSR;

    public NPCView[] EnemyViews { get => EnemySlots.Select(e => (NPCView)(e.Combatant)).Where(c => c != null).ToArray(); }
    public CombatantView[] HeroViews { get => HeroSlots.Select(e => e.Combatant).Where(s => s != null).ToArray(); }
    public bool Dead { get; private set; } = false;
    public BoardView Board { get; private set; }
    public int Index { get; private set; }

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
    
    public SlotView FirstAvailableHeroSlot()
    {
        foreach (SlotView slot in HeroSlots)
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
        for(int i = 0; i < EnemySlots.Length; i++)
        {
            SlotView slot = EnemySlots[i];
            slot.SetUp(this, i);
        }

        for (int i = 0; i < HeroSlots.Length; i++)
        {
            SlotView slot = HeroSlots[i];
            slot.SetUp(this, i);
        }
    }

    public void SetUp(BoardView board, int index)
    {
        Board = board;
        Index = index;
    }

    private void OnDestroy()
    {
        _targetPreviewSR.transform.DOKill();
    }

    public void SlideEnemiesLeft()
    {
        for(int i=0;i<EnemyViews.Length;i++)
        {
            NPCView enemyView = EnemyViews[i];
            SlotView enemySlot = EnemySlots[i];

            if (enemyView.Slot != enemySlot)
            {
                enemySlot.AddCombatant(enemyView, false);
            }
        }
    }

    public void SlideAlliesLeft()
    {
        for (int i = 0; i < HeroViews.Length; i++)
        {
            CombatantView heroView = HeroViews[i];
            SlotView heroSlot = HeroSlots[i];

            if (heroView.Slot != heroSlot)
            {
                heroSlot.AddCombatant(heroView, false);
            }
        }
    }

    public void AddEnemy(NPCView enemyView, LaneView originalLaneView = null)
    {
        SlotView slot = FirstAvailableEnemySlot();

        if (slot == null)
            return;

        slot.AddCombatant(enemyView);

        if (originalLaneView != null)
        {
            MoveUnitsGA moveEnemyGA = new(this, enemyView, null);
            ActionSystem.Instance.AddReaction(moveEnemyGA);
        }
        else
        {
            enemyView.transform.localPosition = Vector3.zero;
        }
    }

    public IEnumerator RemoveEnemy(NPCView enemyView)
    {
        if (!EnemyViews.Contains(enemyView))
            yield break;

        yield return enemyView.WaitForTweensComplete();

        enemyView.Slot.RemoveCombatant();

        Tween tween = enemyView.transform.DOScale(Vector3.zero, 0.25f);
        yield return tween.WaitForCompletion();
        Destroy(enemyView.gameObject);

        SlideEnemiesLeft();

        tween = null;

        foreach (NPCView npc in EnemyViews)
        {
            if (npc.transform.localPosition != Vector3.zero)
            {
                tween = npc.transform.DOLocalMove(Vector3.zero, 0.4f);
            }
        }

        if (tween != null)
            yield return tween.WaitForCompletion();
    }

    public void SetHero(HeroView heroView, LaneView originalLaneView = null)
    {
        if (HeroViews.Length == HeroSlots.Length)
        {
            return;
        }

        FirstAvailableHeroSlot().AddCombatant(heroView);

        if (originalLaneView == null)
            heroView.transform.localPosition = Vector3.zero;
        else
        {
            //TODO: Add move hero functionality
        }
    }

    public IEnumerator SwapHero(HeroView heroView, float duration)
    {
        SlotView heroSlot = FirstAvailableHeroSlot();

        if (heroView != null)
        {
            heroSlot.AddCombatant(heroView, false);
            yield return heroSlot.PullCombatant(0.4f);
        }
    }

    public bool Contains(CombatantView combatant)
    {
        return HeroViews.Contains(combatant) || EnemyViews.Contains(combatant);
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

    public CombatantView FrontHeroView()
    {
        var heroes = HeroViews;

        if (heroes.Length > 0)
            return heroes.Last();

        return null;
    }
    
    public CombatantView FrontEnemyView()
    {
        CombatantView[] enemies = EnemyViews;

        if (enemies.Length > 0)
            return enemies.First();

        return null;
    }
}
