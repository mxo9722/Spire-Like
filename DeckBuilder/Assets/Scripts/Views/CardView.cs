using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class CardView : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text _title;
    [SerializeField] private TMPro.TMP_Text _description;
    [SerializeField] private TMPro.TMP_Text _mana;
    [SerializeField] private SpriteRenderer _imageSR;
    [SerializeField] private GameObject _wrapper;
    [SerializeField] private LayerMask _dropLayer;
    [SerializeField] private SortingGroup _sortingGroup;
    [SerializeField] private Transform _maskTransform;
    [SerializeField] private Transform _bottomOfCardTransform;
    [SerializeField] private ParticleSystem _burnVFX;
    public Card Card { get; private set; }

    private Vector3 dragStartPosition;
    private Quaternion dragStartRotation;

    public void Setup(Card card)
    {
        Card = card;

        _title.text = card.Title;
        _description.text = card.Description;
        _mana.text = card.Mana.ToString();
        _imageSR.sprite = card.Image;
    }

    public void SetBasePos(Vector3 position, Quaternion quat)
    {
        dragStartPosition = position;
        dragStartRotation = quat;
    }

    private void OnMouseEnter()
    {
        if (!Interactions.Instance.PlayerCanHover()) return;
        _wrapper.SetActive(false);
        Vector3 pos = new Vector3(transform.position.x, -2, 0);
        CardViewHoverSystem.Instance.Show(Card, pos);
    }

    private void OnMouseExit()
    {
        if (!Interactions.Instance.PlayerCanHover()) return;
        CardViewHoverSystem.Instance.Hide();
        _wrapper.SetActive(true);
    }

    private void OnMouseDown()
    {
        if (!Interactions.Instance.PlayerCanInteract()) return;
        if (Card.ManualTargetEffect != null)
        {
            ManualTargetSystem.Instance.StartTargeting(transform.position);
        }
        else
        {
            Interactions.Instance.playerIsDragging = true;
            _wrapper.SetActive(true);
            CardViewHoverSystem.Instance.Hide();
            //dragStartPosition = transform.position;
            //dragStartRotation = transform.rotation;
            transform.rotation = Quaternion.identity;
            transform.position = MouseUtil.GetMousePositionInWorldSpace(-1);
        }
    }

    private void OnMouseDrag()
    {
        if (!Interactions.Instance.PlayerCanInteract()) return;
        if (Card.ManualTargetEffect != null) return;
        transform.position = MouseUtil.GetMousePositionInWorldSpace(-1);

    }

    private void OnMouseUp()
    {
        if (!Interactions.Instance.PlayerCanInteract()) return;
        if (Card.ManualTargetEffect != null)
        {
            EnemyView target = ManualTargetSystem.Instance.EndTargeting(MouseUtil.GetMousePositionInWorldSpace(-1));
            if(target != null && ManaSystem.Instance.HasEnoughMana(Card.Mana))
            {
                PlayCardGA playCardGA = new PlayCardGA(Card, target);
                ActionSystem.Instance.Perform(playCardGA);
            }
        }
        else
        {
            if (ManaSystem.Instance.HasEnoughMana(Card.Mana) && Physics.Raycast(transform.position, Vector3.forward, out RaycastHit hit, 10f, _dropLayer))
            {
                PlayCardGA playCardGA = new PlayCardGA(Card);
                ActionSystem.Instance.Perform(playCardGA);
            }
            else
            {
                transform.position = dragStartPosition;
                transform.rotation = dragStartRotation;
            }

            Interactions.Instance.playerIsDragging = false;
        }
    }

    public IEnumerator ActivateExhaustVFX()
    {
        float startStopTime = 0.1f;
        float burnLength = _burnVFX.main.duration;

        _sortingGroup.sortingOrder = 99;

        _burnVFX.gameObject.SetActive(true);

        Debug.Log(burnLength);

        yield return new WaitForSeconds(startStopTime);

        float destroyTime = burnLength - (startStopTime * 2);

        Tweener burnTweener = _burnVFX.transform.DOMove(_bottomOfCardTransform.position-new Vector3(0,1,0), destroyTime);
        _maskTransform.DOMove(_bottomOfCardTransform.position, destroyTime);
        _maskTransform.DOScaleY(0, destroyTime);

        yield return burnTweener.WaitForCompletion();

        yield return new WaitForSeconds(startStopTime);
        
        Destroy(gameObject);
    }
}
