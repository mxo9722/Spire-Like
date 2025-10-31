using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class CombatantView : MonoBehaviour
{
    [SerializeField] private TMP_Text _healthText;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private StatusEffectsUI _statusEffectsUI;
    public int MaxHealth { get; private set; }
    public int CurrentHealth { get; private set; }

    private Dictionary<StatusEffectType, int> _statusEffects = new();

    protected void SetupBase(int health, Sprite sprite)
    {
        MaxHealth = CurrentHealth = health;
        _spriteRenderer.sprite = sprite;
        UpdateHealthText();
    }

    private void UpdateHealthText()
    {
        _healthText.text = "HP: " + CurrentHealth;
    }

    public void Damage(int damageAmount)
    {
        int remainingDamage = damageAmount;
        int currentArmor = GetStatusEffectStacks(StatusEffectType.ARMOR);

        if (currentArmor >= remainingDamage)
        {
            RemoveStatusEffect(StatusEffectType.ARMOR, remainingDamage);
            remainingDamage = 0;
        }
        else if (currentArmor > 0)
        {
            RemoveStatusEffect(StatusEffectType.ARMOR, currentArmor);
            remainingDamage -= currentArmor;
        }

        CurrentHealth -= remainingDamage;

        if (CurrentHealth < 0)
            CurrentHealth = 0;


        UpdateHealthText();

        if (CurrentHealth == 0)
        {
            Die();
        }
        else if (remainingDamage > 0)
        {
            transform.DOShakePosition(0.2f, 0.5f);
        }
    }

    public void AddStatusEffect(StatusEffectType type, int stackCount)
    {
        if (_statusEffects.ContainsKey(type))
        {
            _statusEffects[type] += stackCount;
        }
        else
        {
            _statusEffects.Add(type, stackCount);
        }

        _statusEffectsUI.UpdateStatusEffectsUI(type, _statusEffects[type]);
    }
    
    public void RemoveStatusEffect(StatusEffectType type, int stackCount)
    {
        if (_statusEffects.ContainsKey(type))
        {
            _statusEffects[type] -= stackCount;
            if (_statusEffects[type] < 0)
                _statusEffects[type] = 0;
        }
        else
        {
            _statusEffects.Add(type, stackCount);
        }

        _statusEffectsUI.UpdateStatusEffectsUI(type, _statusEffects[type]);
    }

    public int GetStatusEffectStacks(StatusEffectType statusEffectType)
    {
        if (_statusEffects.ContainsKey(statusEffectType)) return _statusEffects[statusEffectType];
        return 0;
    }

    public abstract void Die();
}
