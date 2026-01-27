using System.Collections.Generic;
using UnityEngine;

public class StatusEffectsUI : MonoBehaviour
{
    [SerializeField] private StatusEffectUI _statusEffectUIPrefab;
    [SerializeField] private StatusEffectsData _statusEffectsData;

    private Dictionary<StatusEffect, StatusEffectUI> _statusEffectUIs = new();

    private CombatantView _owner;

    public void SetUp(CombatantView owner)
    {
        _owner = owner;
    }

    public void UpdateStatusEffectsUI(StatusEffect statusEffectType, int stackCount)
    {
        if (stackCount == 0)
        {
            if (_statusEffectUIs.ContainsKey(statusEffectType))
            {
                StatusEffectUI statusEffectUI = _statusEffectUIs[statusEffectType];
                _statusEffectUIs.Remove(statusEffectType);
                Destroy(statusEffectUI.gameObject);
            }
        }
        else
        {
            if (!_statusEffectUIs.ContainsKey(statusEffectType))
            {
                StatusEffectUI statusEffectUI = Instantiate(_statusEffectUIPrefab, transform);
                _statusEffectUIs.Add(statusEffectType, statusEffectUI);
            }

            Sprite sprite = GetSpriteByType(statusEffectType);

            int displayStackCount = _statusEffectsData.Map[statusEffectType].Stackable ? stackCount : 0;

            _statusEffectUIs[statusEffectType].Set(_owner, sprite, displayStackCount, statusEffectType);
        }
    }

    private Sprite GetSpriteByType(StatusEffect statusEffectType)
    {
        return _statusEffectsData.Map[statusEffectType].Sprite;
    }
}
