using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LaneView : MonoBehaviour, ITargetPreviewable
{
    [field: SerializeField] public SlotView HeroSlot { get; private set; }
    [field: SerializeField] public SlotView[] EnemySlots { get; private set; }

    [SerializeField] private SpriteRenderer _targetPreviewSR;
    [SerializeField] private SpriteRenderer _soakedStatus;

    public NPCView[] EnemyViews { get => EnemySlots.Select(e => (NPCView)(e.Combatant)).Where(c => c != null).ToArray(); }
    public CombatantView HeroView { get => HeroSlot.Combatant; }
    public bool Dead { get; private set; } = false;
    public BoardView Board { get; private set; }
    public int Index { get; private set; }
    public bool IsSoaked { get; private set; } = false;

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
        _soakedStatus.transform.localScale = Vector3.zero;

        _targetPreviewSR.transform.DORotate(new(0, 0, 180.0f), 2f, RotateMode.Fast).SetLoops(-1).SetEase(Ease.Linear);
        HideTargetPreview();
        for (int i = 0; i < EnemySlots.Length; i++)
        {
            SlotView slot = EnemySlots[i];
            slot.SetUp(this, i);
        }

        SlotView heroSlot = HeroSlot;
        heroSlot.SetUp(this, 0);
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
        CombatantView[] enemies = EnemyViews;

        enemies = SortLane(enemies);

        for (int i = 0; i < enemies.Length; i++)
        {
            NPCView enemyView = (NPCView)enemies[i];
            SlotView enemySlot = EnemySlots[i];

            if (enemyView.Slot != enemySlot)
            {
                enemySlot.AddCombatant(enemyView, false);
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

    public IEnumerator RemoveNPC(NPCView npcView)
    {
        if (!EnemyViews.Contains(npcView))
            yield break;

        yield return npcView.WaitForTweensComplete();

        npcView.Slot.RemoveCombatant();

        Tween tween = npcView.transform.DOScale(Vector3.zero, 0.25f);
        yield return tween.WaitForCompletion();
        Destroy(npcView.gameObject);

        SlideEnemiesLeft();

        tween = null;

        foreach (NPCView npc in EnemyViews)
        {
            if (npc.transform.localPosition != Vector3.zero)
            {
                tween = npc.transform.DOLocalMove(Vector3.zero, 0.4f);
            }
        }

        if (HeroView != null && HeroView.transform.localPosition != Vector3.zero)
        {
            tween = HeroView.transform.DOLocalMove(Vector3.zero, 0.4f);
        }

        if (tween != null)
            yield return tween.WaitForCompletion();
    }

    public void SetHero(HeroView heroView, LaneView originalLaneView = null)
    {
        if (HeroView != null)
        {
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
        SlotView heroSlot = HeroSlot;

        if (heroView != null)
        {
            heroSlot.AddCombatant(heroView, false);
            yield return heroSlot.PullCombatant(0.4f);
        }
    }

    public IEnumerator SetSoaked(bool state, float duration = 1)
    {
        if (IsSoaked == state)
            yield break;

        IsSoaked = state;

        _soakedStatus.transform.DOKill();

        Tween tween = null;

        if (state)
            tween = _soakedStatus.transform.DOScale(Vector3.one, duration).SetEase(Ease.OutCirc);
        else
            tween = _soakedStatus.transform.DOScale(Vector3.zero, duration).SetEase(Ease.OutCirc);

        if (tween != null)
        {
            yield return tween.WaitForCompletion();
        }
    }

    public bool Contains(CombatantView combatant)
    {
        return HeroView == combatant || EnemyViews.Contains(combatant);
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

    public CombatantView FrontEnemyView()
    {
        CombatantView[] enemies = EnemyViews;

        if (enemies.Length > 0)
            return enemies.First();

        return null;
    }

    public CombatantView[] GetFriendlyCombatants(CombatantView combatantView)
    {
        if (combatantView is NPCView npc && npc.IsEvil)
            return EnemyViews;

        return new[] { HeroView };
    }

    public int GetFriendlyCount(CombatantView combatantView)
    {
        if (combatantView is NPCView npc && npc.IsEvil)
            return EnemyViews.Length;

        return HeroView == null ? 0 : 1;
    }

    public CombatantView[] SortLane(CombatantView[] npcs)
    {
        List<CombatantView> list = new(npcs);

        list.Sort(
            //(x, y) =>
            //{
            //    return x.GetSortValue() - y.GetSortValue();
            //}
            );

        return list.ToArray();
    }

    public bool IsSelectable()
    {
        return true;
    }
}
