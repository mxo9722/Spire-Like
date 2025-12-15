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


    private void Update()
    {
        Vector3 endPosition = MouseUtil.GetMousePositionInWorldSpace();
        Vector3 direction = -(_startPosition - _arrowHead.transform.position).normalized;
        _lineRenderer.SetPosition(1, endPosition - direction * 0.5f);
        _arrowHead.transform.position = endPosition;
        _arrowHead.transform.right = direction;

        Color color = _defaultColor;

        Collider2D target = ManualTargetSystem.Instance.ValidRaycast(MouseUtil.GetMousePositionInWorldSpace(-1));



        if (target != null)
        {
            bool isValid = false;

            switch (ManualTargetSystem.Instance.ManualTargetType)
            {
                case ManualTargetType.COMBATANT:
                    CombatantView combatantView = target.GetComponent<CombatantView>();
                    isValid = ManualTargetSystem.Instance.CombatantIsValid(EffectContext.CreateHeroEC(combatantView),combatantView);
                    break;
                case ManualTargetType.LANE:
                    LaneView laneView = target.GetComponent<LaneView>();
                    isValid = ManualTargetSystem.Instance.LaneIsValid(EffectContext.CreateHeroEC(laneView), laneView);
                    break;
            }

            color = isValid ? _validHoverColor : _invalidHoverColor;

            if (isValid)
            {
                switch (ManualTargetSystem.Instance.ManualTargetType)
                {
                    case ManualTargetType.COMBATANT:

                        CombatantView ct = target.GetComponent<CombatantView>();
                        CardViewHoverSystem.Instance.UpdateDynamicDescription(ct);

                        if (CardViewHoverSystem.Instance.CardViewHover.IsHighlighted(EffectContext.CreateHeroEC(ct)))
                            color = _highlightHoverColor;
                        break;
                    case ManualTargetType.LANE:
                        LaneView lt = target.GetComponent<LaneView>();
                        CardViewHoverSystem.Instance.UpdateDynamicDescription(lt);

                        if (CardViewHoverSystem.Instance.CardViewHover.IsHighlighted(EffectContext.CreateHeroEC(lt)))
                            color = _highlightHoverColor;
                        break;
                }
            }
            else
            {
                CardViewHoverSystem.Instance.UpdateDynamicDescription();
            }
        }
        else
            CardViewHoverSystem.Instance.UpdateDynamicDescription();

        _lineRenderer.startColor = color;
        _lineRenderer.endColor = color;
        _arrowHead.color = color;
    }

    public void SetupArrow(Vector3 startPosition)
    {
        _startPosition = startPosition;
        _lineRenderer.SetPosition(0, startPosition);
        _lineRenderer.SetPosition(1, MouseUtil.GetMousePositionInWorldSpace());
    }
}
