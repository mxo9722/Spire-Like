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

    private CombatantView _owner;
    private int _stackCount = 0;
    public StatusEffectInfo Info { get; private set; }
    public StatusEffect StatusEffectType { get => Info.EnumKey; }

    private bool _started = false;

    public void Set(CombatantView owner, Sprite sprite, int stackCount, bool canStack, StatusEffectInfo info)
    {
        _owner = owner;
        _image.sprite = sprite;

        if (canStack)
            _stackCountText.text = stackCount.ToString();
        else
            _stackCountText.text = "";

        _stackCount = stackCount;

        Info = info;

        Info = info;

        if (!_started)
        {
            foreach (StatusEffectReaction reaction in Info.Reactions)
            {
                reaction.SubscribeCondition(this, HandleReaction);
            }
            
            foreach (ConditionalModifierPair modifier in Info.Modifiers)
            {
                modifier.Subscribe(this);
            }

            _started = true;
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
    }

    private void OnDisable()
    {
        if (Info != null)
        {
            foreach (StatusEffectReaction reaction in Info.Reactions)
            {
                reaction.UnsubscribeCondition(this, HandleReaction);
            }
            
            foreach (ConditionalModifierPair modifier in Info.Modifiers)
            {
                modifier.Unsubscribe(this);
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

    private void HandleReaction(GameAction gameAction)
    {
        foreach (StatusEffectReaction reaction in Info.Reactions)
        {
            int count = reaction.SubConditionIsMet(_owner, gameAction);

            EffectContext context = new(_owner);

            context.SetData("stacks",_stackCount);
            reaction.SaveTargetData(context, gameAction);

            for (int i = 0; i < count; i++)
            {
                reaction.InvokeEffects(context);
            }
        }
    }

    public void SetPositionOverride(Vector3 position)
    {
        _layoutElement.ignoreLayout = true;
        transform.position = position;
    }
}
