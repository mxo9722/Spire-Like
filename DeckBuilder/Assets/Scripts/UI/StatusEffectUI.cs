using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StatusEffectUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image _image;
    [SerializeField] private TMP_Text _stackCountText;
    [SerializeField] private HelpBoxUI _helpBoxUI;

    private int _stackCount = 0;
    private StatusEffectType _statusEffectType;

    public void Set(Sprite sprite, int stackCount, StatusEffectType statusEffectType)
    {
        _image.sprite = sprite;

        if(stackCount!=0)
            _stackCountText.text = stackCount.ToString();
        else
            _stackCountText.text = "";

        _stackCount = stackCount;

        _statusEffectType = statusEffectType;

        LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        _helpBoxUI.SetUpFromStatusEffect(_statusEffectType, _stackCount);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _helpBoxUI.Hide();
    }
}
