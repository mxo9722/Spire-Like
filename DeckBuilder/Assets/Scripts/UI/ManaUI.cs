using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ManaUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TMP_Text _mana;
    [SerializeField] private Image _img;

    [SerializeField] private HelpBoxUI _helpBox;

    private void OnDisable()
    {
        _img.transform.DOKill();
    }

    public void UpdateManaText(int currentDashes)
    {
        string newValue = currentDashes.ToString();
        if (newValue != _mana.text)
        {
            _mana.text = newValue;

            _img.transform.DOKill(true);
            _img.transform.DOPunchScale(new(1.1f, 1.1f), 0.5f, 1);
        }
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        _helpBox.SetUpFromText("Mana", "You have "+_mana.text+" mana which can be used to play cards. You regain all mana after your turn has ended.");
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        _helpBox.Hide();
    }
}
