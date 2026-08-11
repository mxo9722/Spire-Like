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
    private Card _card;

    public void StartTargeting(Vector3 startPosition, Card card)
    {
        _arrowView.gameObject.SetActive(true);
        _arrowView.SetupArrow(startPosition, card.GetOwnerView());
        ManualTargetType = card.ManualTargetType;
        _combatantFilters = card.CombatantFilters;
        _laneFilters = card.LaneFilters;
        _card = card;

        HighlightManualTargets(card);
    }

    public void HighlightManualTargets(Card card)
    {
        List<ConditionalAutoTargetEffect> highlightConditionals = card.OtherEffects.Where(e => e is ConditionalAutoTargetEffect conditional && !conditional.HideHighlight).Select(e => (ConditionalAutoTargetEffect)e).ToList();

        switch (card.ManualTargetType)
        {
            case ManualTargetType.COMBATANT:
                TargetPreviewSystem.Instance.SetTargetPreviewsManual<CombatantView, CombatantFilter>(card, card.CombatantFilters, highlightConditionals, new(card.GetOwnerView(), playedCard: card));
                break;

            case ManualTargetType.LANE:
                TargetPreviewSystem.Instance.SetTargetPreviewsManual<LaneView, LaneFilter>(card, card.LaneFilters, highlightConditionals, new(card.GetOwnerView(), playedCard: card));
                break;
        }
    }

    public void HighlightTargets(Card card, List<CombatantFilter> filters, EffectContext context)
    {
        TargetPreviewSystem.Instance.SetTargetPreviewsManual<CombatantView, CombatantFilter>(card, filters, new List<ConditionalAutoTargetEffect>(), context);
    }
    
    public void HighlightTargets(Card card,List<LaneFilter> filters, EffectContext context)
    {
        TargetPreviewSystem.Instance.SetTargetPreviewsManual<LaneView, LaneFilter>(card, filters, new List<ConditionalAutoTargetEffect>(), context);
    }

    public CombatantView EndEnemyTargeting(Vector3 endPosition)
    {
        TargetPreviewSystem.Instance.HideTargetPreviews(true);

        if (!_arrowView.gameObject.activeSelf)
            return null;

        _arrowView.gameObject.SetActive(false);
        RaycastHit2D hit = Physics2D.Raycast(endPosition, Vector3.forward, 10f, _enemyTargetLayerMask);
        if (hit.collider != null
            && hit.transform.TryGetComponent(out CombatantView target))
        {
            EffectContext context = new(_card.GetOwnerView(new(null, manualTargetCombatant: target)), manualTargetCombatant:target);

            TargetPreviewSystem.Instance.HideTargetPreviews(true);

            _card = null;

            return CombatantIsValid(context, target) ? target : null;
        }

        _card = null;
        return null;
    }

    public LaneView EndLaneTargeting(Vector3 endPosition)
    {
        TargetPreviewSystem.Instance.HideTargetPreviews(true);

        if (!_arrowView.gameObject.activeSelf && _card != null)
            return null;

        _arrowView.gameObject.SetActive(false);
        RaycastHit2D hit = Physics2D.Raycast(endPosition, Vector3.forward, 10f, _laneTargetLayerMask);
        if (hit.collider != null
            && hit.transform.TryGetComponent(out LaneView target))
        {
            if (_card != null)
            {
                EffectContext context = new(_card.GetOwnerView(new(null, manualTargetLane: target)), manualTargetLane: target);

                TargetPreviewSystem.Instance.HideTargetPreviews(true);

                _card = null;

                return LaneIsValid(context, target) ? target : null;
            }
            else
            {
                return target;
            }
        }

        _card = null;
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
        if (target == null)
            return false;

        if (filters != null)
        {
            if (!target.IsSelectable())
                return false;

            return filters.TargetIsValid(target, context);
        }

        return true;
    }

    public bool LaneIsValid(EffectContext context, LaneView target)
    {
        return LaneIsValid(context, target, _laneFilters);
    }

    public static bool LaneIsValid(EffectContext context, LaneView target, List<LaneFilter> filters)
    {
        if (!target.IsSelectable())
            return false;

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
