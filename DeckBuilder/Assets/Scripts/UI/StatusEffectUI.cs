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

    private CombatantView _owner;
    private int _stackCount = 0;
    private StatusEffect _statusEffectType;

    private bool _started = false;

    public void Set(CombatantView owner, Sprite sprite, int stackCount, StatusEffect statusEffectType)
    {
        _owner = owner;
        _image.sprite = sprite;

        if (stackCount != 0)
            _stackCountText.text = stackCount.ToString();
        else
            _stackCountText.text = "";

        _stackCount = stackCount;

        _statusEffectType = statusEffectType;

        if (!_started)
        {
            StatusEffectInfo info = StatusEffectSystem.Instance.GetStatusEffectInfo(statusEffectType);

            foreach (StatusEffectReaction reaction in info.Reactions)
            {
                reaction.SubscribeCondition(this, HandleReaction);
            }

            _started = true;
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
    }

    private void OnDisable()
    {
        StatusEffectInfo info = StatusEffectSystem.Instance?.GetStatusEffectInfo(_statusEffectType);

        if (info != null)
            foreach (StatusEffectReaction reaction in info.Reactions)
            {
                reaction.UnsubscribeCondition(this, HandleReaction);
            }
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        _helpBoxUI.SetUpFromStatusEffect(_statusEffectType, _stackCount);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _helpBoxUI.Hide();
    }

    private void HandleReaction(GameAction gameAction)
    {
        StatusEffectInfo info = StatusEffectSystem.Instance.GetStatusEffectInfo(_statusEffectType);

        foreach (StatusEffectReaction reaction in info.Reactions)
        {
            int count = reaction.SubConditionIsMet(_owner, gameAction);

            EffectContext context = new(_owner);

            context.AddData("stacks",_stackCount);
            reaction.SaveTargetData(context, gameAction);

            for (int i = 0; i < count; i++)
            {
                reaction.InvokeEffects(context);
            }
        }
    }

}
