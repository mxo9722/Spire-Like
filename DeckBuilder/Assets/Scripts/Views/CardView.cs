using DG.Tweening;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class CardView : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text _title;
    [SerializeField] private TMPro.TMP_Text _description;
    [SerializeField] private TMPro.TMP_Text _mana;
    [SerializeField] private SpriteRenderer _imageSR;
    [SerializeField] private SpriteRenderer _glowSR;
    [SerializeField] private GameObject _wrapper;
    [SerializeField] private LayerMask _dropLayer;
    [SerializeField] private Transform _maskTransform;
    [SerializeField] private Transform _bottomOfCardTransform;
    [SerializeField] private HelpBoxesUI _helpBoxesUI;
    [SerializeField] private ParticleSystem _burnVFX;

    [field: SerializeField] public SortingGroup SortingGroup { get; private set; }


    private int _originalLayerOrder = 0;

    public Card Card { get; private set; }
    public bool Hovering { get; private set; } = false;
    public Action<CardView> OnButtonPressed;

    private Vector3 _dragStartPosition;
    private Quaternion _dragStartRotation;
    private bool _treatAsButton = false;

    public void Setup(Card card, bool treatAsButton = false)
    {
        Card = card;
        _treatAsButton = treatAsButton;

        _originalLayerOrder = SortingGroup.sortingOrder;

        _title.text = card.Title;

        if (!treatAsButton)
            UpdateDynamicDescription(EffectContext.CreateHeroEC());
        else
            UpdateDynamicDescription();

        if (card.Unplayable)
            _mana.text = "";
        else
            _mana.text = card.Mana.ToString();

        Vector2 prevSize = _imageSR.size;

        _imageSR.sprite = card.Image;
        _imageSR.size = prevSize;

        UpdateGlow();
    }

    public void SetBasePos(Vector3 position, Quaternion quat)
    {
        _dragStartPosition = position;
        _dragStartRotation = quat;
    }

    private void OnMouseEnter()
    {
        if (!_treatAsButton)
        {
            if (!Interactions.Instance.PlayerCanHover()) return;

            Hovering = true;

            if (!ManualTargetSystem.Instance.IsTargetting)
            {
                UpdateHoverView();
            }
        }
        else
        {
            _helpBoxesUI.Populate(Card);
        }
    }

    private void OnMouseExit()
    {
        if (!_treatAsButton)
        {
            if (!Interactions.Instance.PlayerCanHover()) return;

            Hovering = false;

            if (!ManualTargetSystem.Instance.IsTargetting)
            {
                CardViewHoverSystem.Instance.Hide();
                _wrapper.SetActive(true);
            }
        }
        else
        {
            _helpBoxesUI.Hide();
        }
    }

    private void OnMouseDown()
    {
        if (!_treatAsButton && !Interactions.Instance.PlayerCanInteract()) return;


        if (!_treatAsButton && IsPlayable())
        {
            Cursor.visible = false;

            if (Card.ManualTargetType != ManualTargetType.NONE && !Card.IsChaotic())
            {
                Vector3 origin = Vector3.zero;
                origin.y = CardViewHoverSystem.Instance.CardViewHover.transform.position.y;
                CardViewHoverSystem.Instance.TweenToPosition(origin);

                ManualTargetSystem.Instance.StartTargeting(origin, Card);
            }
            else
            {
                SortingGroup.sortingOrder = 99;

                Interactions.Instance.playerIsDragging = true;
                _wrapper.SetActive(true);
                CardViewHoverSystem.Instance.Hide();
                //dragStartPosition = transform.position;
                //dragStartRotation = transform.rotation;
                transform.rotation = Quaternion.identity;
                transform.position = MouseUtil.GetMousePositionInWorldSpace(-1);
                TargetPreviewSystem.Instance.SetTargetPreviews(Card);
            }
        }
        else if (_treatAsButton && !CardCollectionSystem.Instance.Opened)
        {
            OnButtonPressed?.Invoke(this);
        }
    }

    private void OnMouseDrag()
    {
        if (_treatAsButton) return;
        if (!Interactions.Instance.PlayerCanInteract()) return;
        if (Card.ManualTargetType != ManualTargetType.NONE && !Card.IsChaotic()) return;
        if (!IsPlayable()) return;
        transform.position = MouseUtil.GetMousePositionInWorldSpace(-1);
    }

    private void OnMouseUp()
    {
        if (_treatAsButton) return;
        if (!Interactions.Instance.PlayerCanInteract()) return;

        Cursor.visible = true;
        bool onDropLayer = Physics.Raycast(transform.position, Vector3.forward, out RaycastHit hit, 10f, _dropLayer);

        if (Card.ManualTargetType != ManualTargetType.NONE)
        {

            EffectContext context = EffectContext.CreateHeroEC();

            switch (Card.ManualTargetType)
            {
                case ManualTargetType.COMBATANT:
                    CombatantView combatantView = null;

                    if (!Card.IsChaotic())
                        combatantView = ManualTargetSystem.Instance.EndEnemyTargeting(MouseUtil.GetMousePositionInWorldSpace(-1));
                    else if (onDropLayer)
                        combatantView = Card.GetChaosTargetMode<CombatantView>().GetTargets(context).FirstOrDefault();

                    if (combatantView != null && IsPlayable())
                    {
                        PlayCardGA playCardGA = new PlayCardGA(Card, combatantView);
                        ActionSystem.Instance.Perform(playCardGA);
                    }
                    break;
                case ManualTargetType.LANE:
                    LaneView laneView = null;

                    if (!Card.IsChaotic())
                        laneView = ManualTargetSystem.Instance.EndLaneTargeting(MouseUtil.GetMousePositionInWorldSpace(-1));
                    else if (onDropLayer)
                        laneView = Card.GetChaosTargetMode<LaneView>().GetTargets(context).FirstOrDefault();
                    
                    if (laneView != null && IsPlayable())
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
        else if (!CardCollectionSystem.Instance.Opened)
        {
            if (IsPlayable() && onDropLayer)
            {
                PlayCardGA playCardGA = new PlayCardGA(Card);
                ActionSystem.Instance.Perform(playCardGA);

                if (Card.ExhuastOnUse)
                {
                    transform.DOMove(_dragStartPosition + new Vector3(0, 2, 0), 0.15f);
                    SortingGroup.sortingOrder++;
                }
            }

        }

        transform.position = _dragStartPosition;
        transform.rotation = _dragStartRotation;

        Interactions.Instance.playerIsDragging = false;
        TargetPreviewSystem.Instance.HideTargetPreviews();

        SortingGroup.sortingOrder = _originalLayerOrder;
    }

    public IEnumerator ActivateExhaustVFX()
    {
        CardViewHoverSystem.Instance.Hide();
        _wrapper.SetActive(true);

        float startStopTime = 0.1f;
        float burnLength = _burnVFX.main.duration;

        SortingGroup.sortingOrder = 99;

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
            Vector3 pos = new(transform.position.x, -2.5f);

            if (_treatAsButton)
                pos = new(transform.position.x, transform.position.y);

            CardViewHoverSystem.Instance.Show(Card, pos);
        }
    }

    public void UpdateDynamicDescription(EffectContext context = null)
    {
        if (context == null || _treatAsButton)
        {
            _description.text = Card.GetStaticDescription();
        }
        else
        {
            _description.text = Card.GetDynamicDescription(context);
        }
    }

    public void SetSortingOrder(int sortingOrder)
    {
        SortingGroup.sortingOrder = sortingOrder;
        _originalLayerOrder = sortingOrder;
    }

    public void UpdateGlow()
    {
        if (_treatAsButton)
            return;

        if (IsPlayable())
        {
            if (IsHighlighted())
                SetGlow(CardSystem.Instance.HighlightColor);
            else
                SetGlow(CardSystem.Instance.PlayableColor);
        }
        else
        {
            SetGlow(Color.clear);
        }
    }

    public void SetGlow(Color color)
    {
        if (_glowSR.color != color)
        {
            float duration = 0.5f;

            //To avoid glow transitioning for the hover card
            if (enabled)
            {
                Vector3 punchScale = new(0.05f, 0.05f);

                _glowSR.transform.DOComplete();
                _glowSR.transform.DOPunchScale(punchScale, duration, 1);

                _glowSR.DOKill();
                _glowSR.DOColor(color, duration);
            }
            else
            {
                _glowSR.color = color;
            }
        }
    }

    public void HideGlow()
    {
        _glowSR.DOKill();
        _glowSR.color = Color.clear;
    }

    public bool IsPlayable(CombatantView caster = null) => Card.IsPlayable();
    public bool IsHighlighted() => Card.IsHighlighted();
    public bool IsHighlighted(EffectContext context) => Card.IsHighlighted(context);
}
