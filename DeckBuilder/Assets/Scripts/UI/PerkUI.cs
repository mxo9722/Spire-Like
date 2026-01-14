using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PerkUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image _image;
    [SerializeField] private HelpBoxUI _helpBoxUI;

    public Perk Perk { get; private set; }

    public void SetUp(Perk perk)
    {
        Perk = perk;
        _image.sprite = perk.Image;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _helpBoxUI.SetUpFromText(Perk.Name, Perk.Description);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _helpBoxUI.Hide();
    }
}
