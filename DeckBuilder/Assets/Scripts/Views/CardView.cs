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

    private int _originalLayerOrder = 0;

    public Card Card { get; private set; }
    public bool Hovering { get; private set; } = false;

    private Vector3 _dragStartPosition;
    private Quaternion _dragStartRotation;

    public void Setup(Card card)
    {
        Card = card;

        _originalLayerOrder = _sortingGroup.sortingOrder;

        _title.text = card.Title;
        UpdateDynamicDescription(TargetModeContext.CreateHeroTMC());
        _mana.text = card.Mana.ToString();
        _imageSR.sprite = card.Image;
    }

    public void SetBasePos(Vector3 position, Quaternion quat)
    {
        _dragStartPosition = position;
        _dragStartRotation = quat;
    }

    private void OnMouseEnter()
    {
        if (!Interactions.Instance.PlayerCanHover()) return;

        Hovering = true;

        if (!ManualTargetSystem.Instance.IsTargetting)
        {
            UpdateHoverView();
        }
    }

    private void OnMouseExit()
    {
        if (!Interactions.Instance.PlayerCanHover()) return;

        Hovering = false;

        if (!ManualTargetSystem.Instance.IsTargetting)
        {
            CardViewHoverSystem.Instance.Hide();
            _wrapper.SetActive(true);
        }
    }

    private void OnMouseDown()
    {
        if (!Interactions.Instance.PlayerCanInteract()) return;

        Cursor.visible = false;

        if (Card.ManualTargetEffect != null)
        {
            Vector3 origin = transform.position;
            origin = BoardSystem.Instance.BoardView.GetCombatantHoverPosition();
            CardViewHoverSystem.Instance.TweenToPosition(origin);

            ManualTargetSystem.Instance.StartTargeting(origin, Card.ManualTargetType);
        }
        else
        {
            _sortingGroup.sortingOrder = 99;

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

        Cursor.visible = true;

        if (Card.ManualTargetEffect != null)
        {
            switch (Card.ManualTargetType)
            {
                case ManualTargetType.ENEMY:
                    EnemyView enemyView = ManualTargetSystem.Instance.EndEnemyTargeting(MouseUtil.GetMousePositionInWorldSpace(-1));
                    if (enemyView != null && ManaSystem.Instance.HasEnoughMana(Card.Mana))
                    {
                        PlayCardGA playCardGA = new PlayCardGA(Card, enemyView);
                        ActionSystem.Instance.Perform(playCardGA);
                    }
                    break;
                case ManualTargetType.LANE:
                    LaneView laneView = ManualTargetSystem.Instance.EndLaneTargeting(MouseUtil.GetMousePositionInWorldSpace(-1));
                    if (laneView != null && ManaSystem.Instance.HasEnoughMana(Card.Mana))
                    {
                        PlayCardGA playCardGA = new PlayCardGA(Card, laneView);
                        ActionSystem.Instance.Perform(playCardGA);
                    }
                    break;
            }

            CardViewHoverSystem.Instance.Hide();
            _wrapper.SetActive(true);

            CardSystem.Instance.UpdateCardHoverView();
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
                transform.position = _dragStartPosition;
                transform.rotation = _dragStartRotation;
            }

            _sortingGroup.sortingOrder = _originalLayerOrder;

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

        Tweener burnTweener = _burnVFX.transform.DOMove(_bottomOfCardTransform.position - new Vector3(0, 1, 0), destroyTime);
        _maskTransform.DOMove(_bottomOfCardTransform.position, destroyTime);
        _maskTransform.DOScaleY(0, destroyTime);

        yield return burnTweener.WaitForCompletion();

        yield return new WaitForSeconds(startStopTime);

        Destroy(gameObject);
    }

    public void UpdateHoverView()
    {
        if (Hovering)
        {
            _wrapper.SetActive(false);
            Vector3 pos = new Vector3(transform.position.x, -2.5f, 0);
            CardViewHoverSystem.Instance.Show(Card, pos);
        }
    }

    public void UpdateDynamicDescription(TargetModeContext targetModeContext)
    {
        HeroView heroView = HeroSystem.Instance.HeroView;

        if (heroView == null)
        {
            _description.text = Card.GetStaticDescription();
            return;
        }

        _description.text = Card.GetDynamicDescription(targetModeContext);
    }
}
