using System.Collections.Generic;
using UnityEngine;

public class ArrowView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _arrowHead;
    [SerializeField] private LineRenderer _lineRenderer;

    [SerializeField] private Color _defaultColor;
    [SerializeField] private Color _validHoverColor;

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
            color = _validHoverColor;
            switch (ManualTargetSystem.Instance.ManualTargetType)
            {
                case ManualTargetType.ENEMY:
                    CardViewHoverSystem.Instance.UpdateDynamicDescription(targetCombatant: GetComponent<EnemyView>());
                    break;
                case ManualTargetType.LANE:
                    CardViewHoverSystem.Instance.UpdateDynamicDescription(targetLane: GetComponent<LaneView>());
                    break;
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
