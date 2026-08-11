using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StatusEffectUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] private Image _image;
    [SerializeField] private TMP_Text _stackCountText;
    [SerializeField] private HelpBoxUI _helpBoxUI;
    [SerializeField] private LayoutElement _layoutElement;

    public CombatantView Owner { get; private set; }
    private int _stackCount = 0;
    public StatusEffectInfo Info { get; private set; }
    public StatusEffect StatusEffectType { get => Info.EnumKey; }

    private bool _started = false;

    public void Set(CombatantView owner, Sprite sprite, int stackCount, bool canStack, StatusEffectInfo info)
    {
        Owner = owner;
        _image.sprite = sprite;

        if (canStack)
            _stackCountText.text = stackCount.ToString();
        else
            _stackCountText.text = "";

        _stackCount = stackCount;

        Info = info;

        if (!_started)
        {
            StatusEffectSystem.Instance.TrySubscribeSEReactions(info);
            
            foreach (ConditionalModifierPair modifier in Info.Modifiers)
            {
                modifier.Subscribe(this, HandleModifier);
            }

            _started = true;
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
    }

    private void OnDisable()
    {
        if (Info != null)
        {
            StatusEffectSystem.Instance.TryUnsubscribeSEReactions(Info, Owner);

            foreach (ConditionalModifierPair modifier in Info.Modifiers)
            {
                modifier.Unsubscribe(this, HandleModifier);
            }
        }
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        _helpBoxUI.SetUpFromStatusEffect(Info, _stackCount);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _helpBoxUI.Hide();
    }

    private int HandleModifier(int oValue, ModifierKey modKey)
    {
        modKey.Context.SetData("seOwner", Owner);
        modKey.Context.SetData("stacks", _stackCount);

        foreach (ConditionalModifierPair modifier in Info.Modifiers)
        {

            oValue = modifier.TestCondition(oValue, modKey);
        }

        return oValue;
    }

    public void SetPositionOverride(Vector3 position)
    {
        _layoutElement.ignoreLayout = true;
        transform.position = position;
    }
}
