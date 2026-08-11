using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Button;

public class CardUI : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text _title;
    [SerializeField] private TMPro.TMP_Text _description;
    [SerializeField] private TMPro.TMP_Text _mana;
    [SerializeField] private Image _image;
    [SerializeField] private Image _background;
    [SerializeField] private GameObject _selectGlow;
    [SerializeField] private Button _button;
    [SerializeField] private CanvasGroup _canvasGroup;


    [SerializeField, Min(0)] private float _appearTime;
    [SerializeField, Min(0)] private float _fadeInTime;

    public ButtonClickedEvent OnClicked { get => _button.onClick; }
    public Card Card { get; private set; }

    private Action<Card> _onSelected = null;
    private bool _selectable = false;
    public bool Selected { get; private set; } = false;

    private Coroutine _makeVisible = null;
    private Tween _fadeTween = null;

    public void SetUp(Card card)
    {
        Card = card;

        _title.text = card.Title;
        _description.text = card.GetStaticDescription();

        if (card.Unplayable)
            _mana.text = "";
        else
            _mana.text = card.GetStaticManaValue().ToString();

        if (card.Owner != null)
            _background.color = card.Owner.Color;
        else
            _background.color = Color.white;

        _image.sprite = card.Image;
    }

    public void SetUp(Card card, Action<Card> onSelected, bool selectable)
    {
        SetUp(card);
        _selectable = selectable;
        _onSelected = onSelected;
    }

    private void OnDisable()
    {
        if (_makeVisible != null)
        {
            StopCoroutine(_makeVisible);
            _makeVisible = null;
        }

        if (_fadeTween != null)
        {
            _fadeTween.Kill(true);
            _fadeTween = null;
        }
    }

    public void OnButtonPressed()
    {
        if (_selectable)
        {
            Selected = !Selected;
            _selectGlow.SetActive(Selected);
        }

        _onSelected?.Invoke(Card);
    }

    public void Select()
    {
        
    }

    public void BeginFadeIn()
    {
        _makeVisible = StartCoroutine(FadeIn());
    }

    public IEnumerator FadeIn()
    {
        _canvasGroup.alpha = 0;

        if (_appearTime > 0)
            yield return new WaitForSeconds(_appearTime);

        _fadeTween = _canvasGroup.DOFade(1, _fadeInTime);

        yield return _fadeTween.WaitForCompletion();

        _makeVisible = null;
        _fadeTween = null;
    }
}
