using System.Collections.Generic;
using UnityEngine;

public class StatusEffectsUI : MonoBehaviour
{
    [SerializeField] private StatusEffectUI _statusEffectUIPrefab;
    [SerializeField] private StatusEffectsData _statusEffectsData;

    private Dictionary<StatusEffectType, StatusEffectUI> _statusEffectUIs = new();

    public void UpdateStatusEffectsUI(StatusEffectType statusEffectType, int stackCount)
    {
        if (stackCount <= 0)
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
            _statusEffectUIs[statusEffectType].Set(sprite, stackCount);
        }
    }

    private Sprite GetSpriteByType(StatusEffectType statusEffectType)
    {
        return _statusEffectsData.Map[statusEffectType].Sprite;
    }
}
