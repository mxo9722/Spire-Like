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
    [SerializeField] private SpriteRenderer _background;
    [SerializeField] private SpriteRenderer _imageSR;
    [SerializeField] private SpriteRenderer _glowSR;
    [SerializeField] private GameObject _wrapper;
    [SerializeField] private LayerMask _dropLayer;
    [SerializeField] private Transform _maskTransform;
    [SerializeField] private Transform _bottomOfCardTransform;
    [SerializeField] private HelpBoxesUI _helpBoxesUI;
    [SerializeField] private CardUI _cardReferenceUI;
    [SerializeField] private ParticleSystem _burnVFX;
    [SerializeField] private Transform _heatWarning;

    [field: SerializeField] public SortingGroup SortingGroup { get; private set; }


    private int _originalLayerOrder = 0;

    public Card Card { get; private set; }
    public bool Hovering { get; private set; } = false;
    public Action<CardView> OnButtonPressed;

    private Vector3 _dragStartPosition;
    private Quaternion _dragStartRotation;
    private bool _treatAsButton = false;
    private bool _hasBeenPlayed = false;
    private bool _showTips = true;

    public void Setup(Card card, bool treatAsButton = false)
    {
        Card = card;
        _treatAsButton = treatAsButton;

        _originalLayerOrder = SortingGroup.sortingOrder;

        if (card.Owner != null)
            _background.color = card.Owner.Color;
        else
            _background.color = Color.white;

        string title = card.Title;
        string[] split = title.Split("_");
        if (split.Length > 1)
            title = title.Substring(title.IndexOf("_") + 1);

        _title.text = title;

        if (!treatAsButton)
            UpdateDynamicDescription(new(card.GetOwnerView(), playedCard: card));
        else
            UpdateDynamicDescription(new(null, playedCard: card));

        if (card.Unplayable)
            _mana.text = "";
        else
        {
            int manaCost = card.GetDynamicManaValue(new(card.GetOwnerView(), playedCard: card));
            _mana.text = manaCost.ToString();
        }

        Vector2 prevSize = _imageSR.size;

        _imageSR.sprite = card.Image;
        _imageSR.size = prevSize;

        if (card.CardReference != null)
        {
            _cardReferenceUI.SetUp(new(card.CardReference, card.Owner));
        }
        _cardReferenceUI.gameObject.SetActive(false);
        

        _heatWarning.gameObject.SetActive(false);
        UpdateGlow();
    }

    public void SetBasePos(Vector3 position, Quaternion quat)
    {
        _dragStartPosition = position;
        _dragStartRotation = quat;
    }

    private void OnEnable()
    {
        //_heatWarning.Play();
    }

    private void OnDisable()
    {
        transform.DOKill();
        _glowSR.DOKill();
        _glowSR.transform.DOKill();
        _maskTransform.DOKill();
        _burnVFX.transform.DOKill();

        //_heatWarning.Stop();
    }

    private void OnMouseEnter()
    {
        if (!_treatAsButton)
        {
            if (!Interactions.Instance.PlayerCanHover()) return;
            if (_hasBeenPlayed) return;

            Hovering = true;

            if (!ManualTargetSystem.Instance.IsTargetting)
            {
                UpdateHoverView();
            }
        }
        else
        {
            if(_showTips)
                _helpBoxesUI.Populate(Card);

            if (Card.CardReference != null)
                _cardReferenceUI.gameObject.SetActive(true);
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
            _cardReferenceUI.gameObject.SetActive(false);
        }
    }

    private void OnMouseDown()
    {
        //if (!_treatAsButton && !) return;


        if (!_treatAsButton && IsPlayable() && Interactions.Instance.PlayerCanInteract())
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

            EffectContext context = new(Card.GetOwnerView());

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
                        _hasBeenPlayed = true;
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
                        _hasBeenPlayed = true;
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
                _hasBeenPlayed = true;

                if (Card.ExhuastOnUse)
                {
                    transform.DOKill();
                    transform.DOMove(_dragStartPosition + new Vector3(0, 2, 0), 0.15f);
                    SortingGroup.sortingOrder++;
                }
            }

        }

        transform.position = _dragStartPosition;
        transform.rotation = _dragStartRotation;

        Interactions.Instance.playerIsDragging = false;
        TargetPreviewSystem.Instance.HideTargetPreviews(true);

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
        if (_treatAsButton)
        {
            _description.text = Card.GetStaticDescription();
        }
        else
        {
            if (context == null)
                context = new(null, playedCard: Card);

            _description.text = Card.GetDynamicDescription(context);

            if (Card.Unplayable)
                _mana.text = "";
            else
            {
                int manaCost = Card.GetDynamicManaValue(context);
                _mana.text = manaCost.ToString();
            }
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
            if (IsHighlighted(new(Card.GetOwnerView(), playedCard: Card)))
                SetGlow(CardSystem.Instance.HighlightColor);
            else
                SetGlow(CardSystem.Instance.PlayableColor);
        }
        else
        {
            SetGlow(Color.clear);
        }

        bool heatWarning = Card.IsHeatWarning(new(Card.GetOwnerView(),playedCard: Card));
        _heatWarning.gameObject.SetActive(heatWarning);
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

    public void SetTreatAsButton(bool value)
    {
        _treatAsButton = value;
    }

    public void SetSideUp(bool faceUp, float animTime = 0)
    {
        Vector3 rotation = new(0, faceUp ? 0 : 180, 0);

        if (animTime > 0)
            _wrapper.transform.DOLocalRotate(rotation, animTime);
        else
            _wrapper.transform.localEulerAngles = rotation;

        _showTips = faceUp;
    }

    public bool IsPlayable(CombatantView caster = null) => Card.IsPlayable(caster);
    public bool IsHighlighted(EffectContext context) => Card.IsHighlighted(context);
}
