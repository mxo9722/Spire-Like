using DG.Tweening;
using UnityEngine;

public class HeroView : CombatantView
{

    public virtual bool IsProtagonist { get => true; }
    public Hero Hero { get; private set; }

    private bool _isDragged = false;

    public void Setup(Hero hero, SlotView slot)
    {
        SetupBase(hero.StartingMaxHealth, hero.Image, slot);

        Hero = hero;
        SetHealth(hero.CurrentHealth);

        OnHealthChanged += TopBarUI.Instance.UpdateHealth;
    }

    private void OnDisable()
    {

    }

    private void OnMouseDown()
    {
        if (!Interactions.Instance.PlayerCanInteract()) return;
        if (DashSystem.Instance == null) return;
        if (DashSystem.Instance.CurrentDashes == 0) return;
        if (GetStatusEffectStacks(StatusEffect.HAMSTRUNG) > 0) return;
        _isDragged = true;

    }

    private void OnMouseDrag()
    {
        if (!Interactions.Instance.PlayerCanInteract()) return;
        if (!_isDragged) return;
        transform.DOKill(true);
        transform.position = MouseUtil.GetMousePositionInWorldSpace(transform.position.z);
    }

    private void OnMouseUp()
    {
        if (!_isDragged) return;

        LaneView laneView = ManualTargetSystem.Instance.EndLaneTargeting(MouseUtil.GetMousePositionInWorldSpace(-1));

        if (laneView != null && laneView.HeroView != this)
        {
            DashGA dashGA = new(this, laneView);

            ActionSystem.Instance.Perform(dashGA);
        }
        else
        {
            transform.DOLocalMove(Vector3.zero, 0.3f);
        }

        _isDragged = false;
    }

    public override void Die()
    {
        
    }
}
