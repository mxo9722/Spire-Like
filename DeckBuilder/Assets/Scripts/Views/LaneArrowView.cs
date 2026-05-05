using UnityEngine;

public class LaneArrowView : MonoBehaviour
{

    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private TMPro.TMP_Text _text;
    [SerializeField] private Transform _arrowOrigin;

    private Vector3 _targetPos = default;
    private LaneView _owner;

    public void SetUp(LaneView owner)
    {
        _owner = owner;
    }

    public void UpdateView()
    {
        if (!_owner.HeroSlot.IsEmpty)
        {
            RemoveTarget();
            return;
        }

        foreach (NPCView enemy in _owner.EnemyViews)
        {
            foreach (LaneView laneView in BoardSystem.Instance.GetAllLanes())
            {
                if (laneView == _owner) continue;
                if (!enemy.IsHostileTargetLane(laneView)) continue;

                SetTarget(laneView.IncomingDamageView.transform.position, EnemySystem.Instance.IndirectReduction);
                return;
            }
        }

        RemoveTarget();
    }

    public void SetTarget(Vector3 targetPos, float percent)
    {
        _text.gameObject.SetActive(true);
        _lineRenderer.gameObject.SetActive(true);

        _targetPos = targetPos;

        Vector3[] line = new Vector3[2];

        line[0] = _arrowOrigin.position;
        line[1] = _targetPos;

        line[1].z = line[0].z;

        line[1] += (line[0] - line[1]).normalized * 0.5f;

        _lineRenderer.SetPositions(line);

        Vector3 textPos = (line[0] + line[1]) / 2.0f;
        textPos.z -= 1;
        _text.transform.position = textPos;
        _text.text = Mathf.Round(percent * 100) + "%";
    }

    public void RemoveTarget()
    {
        _text.gameObject.SetActive(false);
        _lineRenderer.gameObject.SetActive(false);
    }
}
