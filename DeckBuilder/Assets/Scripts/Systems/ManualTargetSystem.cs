using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ManualTargetSystem : Singleton<ManualTargetSystem>
{
    public bool IsTargetting { get => _arrowView.gameObject.activeSelf; }

    [SerializeField] private ArrowView _arrowView;
    [SerializeField] private LayerMask _enemyTargetLayerMask;
    [SerializeField] private LayerMask _laneTargetLayerMask;

    public ManualTargetType ManualTargetType { get; private set; } = ManualTargetType.COMBATANT;

    private List<CombatantFilter> _combatantFilters = null;
    private List<LaneFilter> _laneFilters = null;

    public void StartTargeting(Vector3 startPosition, Card card)
    {
        _arrowView.gameObject.SetActive(true);
        _arrowView.SetupArrow(startPosition);
        ManualTargetType = card.ManualTargetType;
        _combatantFilters = card.CombatantFilters;
        _laneFilters = card.LaneFilters;

        HighlightManualTargets(card);
    }

    public void HighlightManualTargets(Card card)
    {
        List<ConditionalAutoTargetEffect> highlightConditionals = card.OtherEffects.Where(e => e is ConditionalAutoTargetEffect).Select(e => (ConditionalAutoTargetEffect)e).ToList();

        switch (card.ManualTargetType)
        {
            case ManualTargetType.COMBATANT:
                TargetPreviewSystem.Instance.SetTargetPreviewsManual<CombatantView, CombatantFilter>(card.CombatantFilters, highlightConditionals);
                break;

            case ManualTargetType.LANE:
                TargetPreviewSystem.Instance.SetTargetPreviewsManual<LaneView, LaneFilter>(card.LaneFilters, highlightConditionals);
                break;
        }
    }

    public CombatantView EndEnemyTargeting(Vector3 endPosition)
    {
        TargetPreviewSystem.Instance.HideTargetPreviews();

        if (!_arrowView.gameObject.activeSelf)
            return null;

        _arrowView.gameObject.SetActive(false);
        RaycastHit2D hit = Physics2D.Raycast(endPosition, Vector3.forward, 10f, _enemyTargetLayerMask);
        if (hit.collider != null
            && hit.transform.TryGetComponent(out CombatantView target))
        {
            EffectContext context = EffectContext.CreateHeroEC(target);

            TargetPreviewSystem.Instance.HideTargetPreviews();

            return CombatantIsValid(context, target) ? target : null;
        }

        return null;
    }

    public LaneView EndLaneTargeting(Vector3 endPosition)
    {
        TargetPreviewSystem.Instance.HideTargetPreviews();

        if (!_arrowView.gameObject.activeSelf)
            return null;

        _arrowView.gameObject.SetActive(false);
        RaycastHit2D hit = Physics2D.Raycast(endPosition, Vector3.forward, 10f, _laneTargetLayerMask);
        if (hit.collider != null
            && hit.transform.TryGetComponent(out LaneView target))
        {
            EffectContext context = EffectContext.CreateHeroEC(target);

            TargetPreviewSystem.Instance.HideTargetPreviews();

            return LaneIsValid(context, target) ? target : null;
        }

        return null;
    }

    public Collider2D ValidRaycast(Vector3 endPosition)
    {
        RaycastHit2D hit = default;

        switch (ManualTargetType)
        {
            case ManualTargetType.COMBATANT:
                hit = Physics2D.Raycast(endPosition, Vector3.forward, 10f, _enemyTargetLayerMask);
                break;
            case ManualTargetType.LANE:
                hit = Physics2D.Raycast(endPosition, Vector3.forward, 10f, _laneTargetLayerMask);
                break;
        }

        return hit.collider;
    }

    public bool CombatantIsValid(EffectContext context, CombatantView target)
    {
        return CombatantIsValid(context, target, _combatantFilters);
    }

    public static bool CombatantIsValid(EffectContext context, CombatantView target, List<CombatantFilter> filters)
    {
        if (filters != null)
        {
            foreach (CombatantFilter filter in filters)
            {
                if (!filter.TestTarget(context, target))
                    return false;
            }
        }

        return true;
    }

    public bool LaneIsValid(EffectContext context, LaneView target)
    {
        return LaneIsValid(context, target, _laneFilters);
    }

    public static bool LaneIsValid(EffectContext context, LaneView target, List<LaneFilter> filters)
    {
        if (filters != null)
        {
            foreach (LaneFilter filter in filters)
            {
                if (!filter.TestTarget(context, target))
                    return false;
            }
        }

        return true;
    }
}
