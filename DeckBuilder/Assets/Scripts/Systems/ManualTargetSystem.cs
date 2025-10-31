using UnityEngine;

public class ManualTargetSystem : Singleton<ManualTargetSystem>
{
    [SerializeField] private ArrowView _arrowView;
    [SerializeField] private LayerMask _targetLayerMask;

    public void StartTargeting(Vector3 startPosition)
    {
        _arrowView.gameObject.SetActive(true);
        _arrowView.SetupArrow(startPosition);
    }

    public EnemyView EndTargeting(Vector3 endPosition)
    {
        _arrowView.gameObject.SetActive(false);
        RaycastHit2D hit = Physics2D.Raycast(endPosition, Vector3.forward, 10f, _targetLayerMask);
        if( hit.collider != null
            && hit.transform.TryGetComponent<EnemyView>(out EnemyView enemyView))
        {
            return enemyView;
        }

        return null;
    }
}
