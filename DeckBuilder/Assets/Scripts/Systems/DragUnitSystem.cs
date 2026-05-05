using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DragUnitSystem : Singleton<DragUnitSystem>
{

    [SerializeField] private TMP_Text _commandText;

    public bool CanDragUnits { get; private set; } = false;
    public List<CombatantFilter> CombatantFilters { get; private set; } = null;
    public List<LaneFilter> LaneFilters { get; private set; } = null;
    public EffectContext EffectContext { get; private set; } = null;

    public CombatantView MovedUnit { get; private set; }
    public LaneView DestinationLane { get; private set; }

    private CardView _cardView;

    public void BeginDrag(List<CombatantFilter> combatantFilters, List<LaneFilter> laneFilters, EffectContext context, string commandText)
    {
        CombatantFilters = combatantFilters;
        LaneFilters = laneFilters;
        EffectContext = context;

        _commandText.text = commandText;

        MovedUnit = null;
        DestinationLane = null;

        CanDragUnits = true;
        HighlightValidUnits();
    }

    public IEnumerator EndDrag(CombatantView combatant, LaneView destination)
    {
        MovedUnit = combatant;
        DestinationLane = destination;

        CombatantFilters = null;
        LaneFilters = null;
        EffectContext = null;

        _commandText.text = "";

        CanDragUnits = false;
        TargetPreviewSystem.Instance.HideTargetPreviews(false);

        _cardView = null;
        yield return null;
    }

    public void HighlightValidUnits()
    {
        TargetPreviewSystem.Instance.HideTargetPreviews(false);
        ManualTargetSystem.Instance.HighlightTargets(CombatantFilters, EffectContext);
    }

    public void HighlightValidLanes(CombatantView view)
    {
        EffectContext context = new(EffectContext);

        context.SetManualCombatant(view);

        TargetPreviewSystem.Instance.HideTargetPreviews(false);
        ManualTargetSystem.Instance.HighlightTargets(LaneFilters, context);
    }
}
