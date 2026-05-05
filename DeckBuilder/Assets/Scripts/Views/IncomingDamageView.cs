using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class IncomingDamageView : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text _text;
    [SerializeField] private SpriteRenderer _background;

    private Vector3 _noHeroScale = new(0.75f, 0.75f);

    private LaneView _owner = null;

    public void SetUp(LaneView owner)
    {
        _owner = owner;
        UpdateView();
    }

    private void OnDisable()
    {
        transform.DOKill();
    }

    public virtual void OnMouseEnter()
    {
        TargetPreviewSystem.Instance.SetEnemyLaneAttackPreview(_owner);
    }

    public virtual void OnMouseExit()
    {
        TargetPreviewSystem.Instance.HideTargetPreviews(false);
    }

    public void UpdateView()
    {
        if (_owner == null)
            return;

        List<NPCView> enemies = BoardSystem.Instance.GetAllEnemies();

        int incomingDamage = 0;

        foreach(NPCView enemy in enemies)
        {
            incomingDamage += enemy.GetTotalDamage(_owner, _owner.HeroView);
        }

        _background.gameObject.SetActive(incomingDamage > 0);

        if (incomingDamage == 0)
            _text.text = "";
        else
            _text.text = incomingDamage.ToString();

        Vector3 expectedScale = Vector3.one;

        if (_owner.HeroView == null)
            expectedScale = _noHeroScale;

        transform.DOKill();

        if (transform.localScale != expectedScale)
        {
            transform.DOScale(expectedScale, 0.25f);
        }
    }
}
