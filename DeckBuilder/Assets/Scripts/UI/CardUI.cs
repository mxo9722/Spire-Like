using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Button;

public class CardUI : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text _title;
    [SerializeField] private TMPro.TMP_Text _description;
    [SerializeField] private TMPro.TMP_Text _mana;
    [SerializeField] private Image _image;
    [SerializeField] private Button _button;

    public ButtonClickedEvent OnClicked { get => _button.onClick; }
    public Card Card { get; private set; }

    public void SetUp(Card card)
    {
        Card = card;

        _title.text = card.Title;
        _description.text = card.Description;
        _mana.text = card.Mana.ToString();
        _image.sprite = card.Image;
    }
}
