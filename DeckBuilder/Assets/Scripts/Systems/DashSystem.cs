using System;
using System.Collections;
using UnityEngine;

public class DashSystem : Singleton<DashSystem>
{
    [SerializeField] private DashUI _dashUI;
    [SerializeField] private int _dashesPerTurn = 1;
    public int CurrentDashes { get; private set; }

    private void OnEnable()
    {
        CurrentDashes = _dashesPerTurn;

        ActionSystem.AttachPerformer<DashGA>(DashPerformer);
        ActionSystem.AttachPerformer<RefillDashesGA>(RefillDashesPerformer);
        ActionSystem.AttachPerformer<SpendDashesGA>(SpendDashesPerformer);
    }

    private void OnDisable()
    {
        ActionSystem.DetachPerformer<DashGA>();
        ActionSystem.DetachPerformer<RefillDashesGA>();
        ActionSystem.DetachPerformer<SpendDashesGA>();
    }

    private IEnumerator DashPerformer(DashGA dashGA)
    {
        MoveUnitsGA moveUnitsGA = new(dashGA.HeroView);
        moveUnitsGA.AddMove(dashGA.HeroView, dashGA.Destination);
        ActionSystem.Instance.AddReaction(moveUnitsGA);

        SpendDashesGA spendDashesGA = new();
        ActionSystem.Instance.AddReaction(spendDashesGA);
        yield return null;
    }

    private IEnumerator RefillDashesPerformer(RefillDashesGA refillDashesGA)
    {
        CurrentDashes = _dashesPerTurn;

        UpdateDashText();

        yield return null;
    }

    private IEnumerator SpendDashesPerformer(SpendDashesGA spendDashesGA)
    {
        CurrentDashes -= spendDashesGA.Amount;
        CurrentDashes = Math.Max(CurrentDashes, 0);

        UpdateDashText();

        yield return null;
    }

    public void UpdateDashText()
    {
        _dashUI.UpdateDashesText(CurrentDashes);
    }
}
