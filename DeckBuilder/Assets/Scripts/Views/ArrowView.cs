using System.Collections.Generic;
using UnityEngine;

public class ArrowView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _arrowHead;
    [SerializeField] private LineRenderer _lineRenderer;

    [SerializeField] private Color _defaultColor;
    [SerializeField] private Color _validHoverColor;
    [SerializeField] private Color _invalidHoverColor;
    [SerializeField] private Color _highlightHoverColor;

    private Vector3 _startPosition;
    private CombatantView _caster;



    public void SetupArrow(Vector3 startPosition, CombatantView caster)
    {
        _startPosition = startPosition;
        _lineRenderer.SetPosition(0, startPosition);
        _lineRenderer.SetPosition(1, MouseUtil.GetMousePositionInWorldSpace());

        _caster = caster;
    }

    private void Update()
    {
        Vector3 endPosition = MouseUtil.GetMousePositionInWorldSpace();
        Vector3 direction = -(_startPosition - _arrowHead.transform.position).normalized;
        _lineRenderer.SetPosition(1, endPosition - direction * 0.5f);
        _arrowHead.transform.position = endPosition;
        _arrowHead.transform.right = direction;

        Color color = _defaultColor;

        Collider2D target = ManualTargetSystem.Instance.ValidRaycast(MouseUtil.GetMousePositionInWorldSpace(-1));

        Card card = CardViewHoverSystem.Instance.CardViewHover.Card;

        if (target != null)
        {
            bool isValid = false;

            switch (ManualTargetSystem.Instance.ManualTargetType)
            {
                case ManualTargetType.COMBATANT:
                    CombatantView combatantView = target.GetComponent<CombatantView>();
                    isValid = ManualTargetSystem.Instance.CombatantIsValid( new( _caster, manualTargetCombatant: combatantView, playedCard: card), combatantView);
                    break;
                case ManualTargetType.LANE:
                    LaneView laneView = target.GetComponent<LaneView>();
                    isValid = ManualTargetSystem.Instance.LaneIsValid( new( _caster, manualTargetLane: laneView, playedCard: card), laneView);
                    break;
            }

            color = isValid ? _validHoverColor : _invalidHoverColor;

            if (isValid)
            {

                switch (ManualTargetSystem.Instance.ManualTargetType)
                {
                    case ManualTargetType.COMBATANT:

                        CombatantView ct = target.GetComponent<CombatantView>();

                        EffectContext cContext = new EffectContext(_caster, manualTargetCombatant: ct, playedCard: card);

                        CardViewHoverSystem.Instance.UpdateDynamicDescription(cContext);

                        TargetPreviewSystem.Instance.SetTargetPreviewsManual(card, ct);

                        if (card.IsHighlighted(cContext))
                            color = _highlightHoverColor;
                        break;
                    case ManualTargetType.LANE:
                        LaneView lt = target.GetComponent<LaneView>();
                        EffectContext lContext = new EffectContext(_caster, manualTargetLane: lt, playedCard: card);

                        CardViewHoverSystem.Instance.UpdateDynamicDescription(lContext);

                        TargetPreviewSystem.Instance.SetTargetPreviewsManual(card, lt);

                        if (card.IsHighlighted(lContext))
                            color = _highlightHoverColor;
                        break;
                }
            }
            else
            {
                CardViewHoverSystem.Instance.UpdateDynamicDescription(card);
                ManualTargetSystem.Instance.HighlightManualTargets(card);
            }
        }
        else
        {
            CardViewHoverSystem.Instance.UpdateDynamicDescription(card);

            ManualTargetSystem.Instance.HighlightManualTargets(card);
        }
        _lineRenderer.startColor = color;
        _lineRenderer.endColor = color;
        _arrowHead.color = color;
    }
}
