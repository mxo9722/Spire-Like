using System;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Button;

public class CardUI : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text _title;
    [SerializeField] private TMPro.TMP_Text _description;
    [SerializeField] private TMPro.TMP_Text _mana;
    [SerializeField] private Image _image;
    [SerializeField] private GameObject _selectGlow;
    [SerializeField] private Button _button;

    public ButtonClickedEvent OnClicked { get => _button.onClick; }
    public Card Card { get; private set; }

    private Action<Card> _onSelected = null;
    private bool _selectable = false;
    public bool Selected { get; private set; } = false; 

    public void SetUp(Card card)
    {
        Card = card;

        _title.text = card.Title;
        _description.text = card.GetStaticDescription();
        _mana.text = card.Mana.ToString();
        _image.sprite = card.Image;
    }

    public void SetUp(Card card, Action<Card> onSelected)
    {
        SetUp(card);
        _selectable = true;
        _onSelected = onSelected;
    }

    public void OnButtonPressed()
    {
        if (_selectable)
        {
            Selected = !Selected;
            _selectGlow.SetActive(Selected);
            _onSelected.Invoke(Card);
        }
    }
}
