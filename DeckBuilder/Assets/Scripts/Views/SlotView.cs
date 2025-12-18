using DG.Tweening;
using System.Collections;
using UnityEngine;

public class SlotView : MonoBehaviour, ITargetPreviewable
{
    [SerializeField] private SpriteRenderer _targetPreviewSR;

    public CombatantView Combatant { get; private set; } = null;

    public bool IsEmpty { get => Combatant == null; }
    public bool TargetPreviewActive => _targetPreviewSR.color != Color.clear;


    private void OnEnable()
    {
        _targetPreviewSR.transform.DORotate(new(0, 0, 180.0f), 2f, RotateMode.Fast).SetLoops(-1).SetEase(Ease.Linear);
        HideTargetPreview();
    }

    public void AddCombatant(CombatantView combatant, bool updatePos = true)
    {
        if (Combatant != null)
            return;

        combatant.Slot?.RemoveCombatant();
        Combatant = combatant;
        combatant.transform.parent = transform;
        combatant.SetSlot(this);

        if (updatePos)
        {
            combatant.transform.position = transform.position;
        }
    }

    public IEnumerator PullCombatant(float duration)
    {
        if (Combatant == null)
            yield break;

        yield return Combatant.WaitForTweensComplete();
        yield return Combatant.transform.DOLocalMove(Vector3.zero, duration);
    }

    public void RemoveCombatant()
    {
        Combatant = null;
    }

    public void SetTargetPreview(Color color)
    {
        if (_targetPreviewSR.color == Color.clear)
        {
            _targetPreviewSR.color = color;
        }
    }

    public void HideTargetPreview()
    {
        _targetPreviewSR.color = Color.clear;
    }
}
