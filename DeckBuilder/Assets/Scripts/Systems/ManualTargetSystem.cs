using UnityEngine;

public class ManualTargetSystem : Singleton<ManualTargetSystem>
{
    public bool IsTargetting { get => _arrowView.gameObject.activeSelf; }
    
    [SerializeField] private ArrowView _arrowView;
    [SerializeField] private LayerMask _enemyTargetLayerMask;
    [SerializeField] private LayerMask _laneTargetLayerMask;

    public ManualTargetType ManualTargetType { get; private set; } = ManualTargetType.ENEMY;

    public void StartTargeting(Vector3 startPosition,ManualTargetType manualTargetType)
    {
        _arrowView.gameObject.SetActive(true);
        _arrowView.SetupArrow(startPosition);
        ManualTargetType = manualTargetType;
    }

    public EnemyView EndEnemyTargeting(Vector3 endPosition)
    {
        _arrowView.gameObject.SetActive(false);
        RaycastHit2D hit = Physics2D.Raycast(endPosition, Vector3.forward, 10f, _enemyTargetLayerMask);
        if( hit.collider != null
            && hit.transform.TryGetComponent(out EnemyView enemyView))
        {
            return enemyView;
        }

        return null;
    }

    public LaneView EndLaneTargeting(Vector3 endPosition)
    {
        _arrowView.gameObject.SetActive(false);
        RaycastHit2D hit = Physics2D.Raycast(endPosition, Vector3.forward, 10f, _laneTargetLayerMask);
        if (hit.collider != null
            && hit.transform.TryGetComponent(out LaneView laneVeiw))
        {
            return laneVeiw;
        }

        return null;
    }

    public Collider2D ValidRaycast(Vector3 endPosition)
    {
        RaycastHit2D hit = default;

        switch (ManualTargetType)
        {
            case ManualTargetType.ENEMY:
                hit = Physics2D.Raycast(endPosition, Vector3.forward, 10f, _enemyTargetLayerMask);
                break;
            case ManualTargetType.LANE:
                hit = Physics2D.Raycast(endPosition, Vector3.forward, 10f, _laneTargetLayerMask);
                break;
        }

        return hit.collider;
    }
}
