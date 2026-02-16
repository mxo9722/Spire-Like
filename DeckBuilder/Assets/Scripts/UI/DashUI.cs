using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DashUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TMP_Text _dash;
    [SerializeField] private Image _img;

    [SerializeField] private HelpBoxUI _helpBox;

    private void OnDisable()
    {
        _img.transform.DOKill();
    }

    public void UpdateDashesText(int currentDashes)
    {
        string newValue = currentDashes.ToString();
        if (newValue != _dash.text) 
        {
            _dash.text = newValue;

            _img.transform.DOKill(true);
            _img.transform.DOPunchScale(new(1.1f,1.1f), 0.5f, 1);
        }
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        _helpBox.SetUpFromText("Dashes", "You can move your heros to other lanes " + _dash.text + " times this turn.");
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        _helpBox.Hide();
    }
}
