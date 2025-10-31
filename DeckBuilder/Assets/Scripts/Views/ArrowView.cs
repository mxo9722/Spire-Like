using UnityEngine;

public class ArrowView : MonoBehaviour
{
    [SerializeField] private GameObject _arrowHead;
    [SerializeField] private LineRenderer _lineRenderer;

    private Vector3 _startPosition;

    private void Update()
    {
        Vector3 endPosition = MouseUtil.GetMousePositionInWorldSpace();
        Vector3 direction = -(_startPosition - _arrowHead.transform.position).normalized;
        _lineRenderer.SetPosition(1, endPosition - direction * 0.5f);
        _arrowHead.transform.position = endPosition;
        _arrowHead.transform.right = direction;
    }

    public void SetupArrow(Vector3 startPosition)
    {
        _startPosition = startPosition;
        _lineRenderer.SetPosition(0, startPosition);
        _lineRenderer.SetPosition(1, MouseUtil.GetMousePositionInWorldSpace());
    }
}
