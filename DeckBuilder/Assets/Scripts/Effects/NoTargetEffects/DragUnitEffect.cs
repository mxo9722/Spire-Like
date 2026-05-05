using SerializeReferenceEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DragUnitEffect : NoTargetEffect, INeedsUserInput
{

    [SerializeField, TextArea(2, 4)] private string _actionText = "Drag unit to new lane";
    [SerializeField] private string _unitKey = "unit";
    [SerializeField] private string _laneKey = "lane";

    [SerializeReference, SR] private List<CombatantFilter> CombatantFilters;
    [SerializeReference, SR] private List<LaneFilter> LaneFilters;

    public IEnumerator WaitForUserInput(EffectContext context)
    {
        DragUnitSystem.Instance.BeginDrag(CombatantFilters, LaneFilters, context, _actionText);

        while (DragUnitSystem.Instance.CanDragUnits)
            yield return new WaitForSeconds(0.1f);

        context.SetData(_unitKey, new List<CombatantView>() { DragUnitSystem.Instance.MovedUnit });
        context.SetData(_laneKey, new List<LaneView>() { DragUnitSystem.Instance.DestinationLane });
    }

    protected override GameAction GetGameAction(EffectContext context)
    {
        MoveUnitsGA moveUnitsGA;
        if (context.Caster == null)
            moveUnitsGA = new MoveUnitsGA(DragUnitSystem.Instance.MovedUnit);
        else
            moveUnitsGA = new MoveUnitsGA(context.Caster);

        moveUnitsGA.AddMove(DragUnitSystem.Instance.MovedUnit, DragUnitSystem.Instance.DestinationLane);

        return moveUnitsGA;
    }
}
