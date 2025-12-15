using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Linq;
using UnityEngine;
using System;

public abstract class CombatantView : MonoBehaviour, ITargetPreviewable
{
    [SerializeField] private TMP_Text _healthText;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private SpriteRenderer _targetPreviewSR;
    [SerializeField] private StatusEffectsUI _statusEffectsUI;
    [SerializeField] private HelpBoxesUI _helpBoxesUI;

    public int MaxHealth { get; private set; }
    public int CurrentHealth { get; private set; }

    public bool TargetPreviewActive => _targetPreviewSR.color != Color.clear;

    private Dictionary<StatusEffectType, int> _statusEffects = new();

    public Action<int> OnHealthChanged;
    public Action<int> OnMaxHealthChanged;

    protected void SetupBase(int health, Sprite sprite)
    {
        MaxHealth = CurrentHealth = health;
        _spriteRenderer.sprite = sprite;
        UpdateHealthText();

        _targetPreviewSR.transform.DORotate(new(0, 0, 180.0f), 2f, RotateMode.Fast).SetLoops(-1).SetEase(Ease.Linear);
        HideTargetPreview();
    }

    public void OnMouseEnter()
    {
        if(!ManualTargetSystem.Instance.IsTargetting && !CardCollectionSystem.Instance.Opened)
            LoadHelpBoxes(_helpBoxesUI);
    }

    public void OnMouseExit()
    {
        _helpBoxesUI.Hide();
    }

    protected virtual void LoadHelpBoxes(HelpBoxesUI helpBoxesUI)
    {
        List<StatusEffectType> allStatusEffects = GetAllActiveStatusEffects();

        foreach (StatusEffectType statusEffect in allStatusEffects)
        {
            _helpBoxesUI.AddHelpBoxFromStatusEffect(statusEffect, _statusEffects[statusEffect]);
        }
    }

    private void UpdateHealthText()
    {
        _healthText.text = "HP: " + CurrentHealth;
    }

    public void Damage(int damageAmount)
    {
        int remainingDamage = damageAmount;
        int currentArmor = GetStatusEffectStacks(StatusEffectType.BLOCK);

        if (remainingDamage == 0)
            return;

        if (currentArmor >= remainingDamage)
        {
            RemoveStatusEffect(StatusEffectType.BLOCK, remainingDamage);
            remainingDamage = 0;
        }
        else if (currentArmor > 0)
        {
            RemoveStatusEffect(StatusEffectType.BLOCK, currentArmor);
            remainingDamage -= currentArmor;
        }

        CurrentHealth -= remainingDamage;

        if (CurrentHealth < 0)
            CurrentHealth = 0;

        OnHealthChanged?.Invoke(CurrentHealth);
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

        _statusEffectsUI.UpdateStatusEffectsUI(type, _statusEffects[type]);
    }

    public List<StatusEffectType> GetAllActiveStatusEffects()
    {
        List<StatusEffectType> allTypes = new(_statusEffects.Keys);

        return allTypes.FindAll(e => _statusEffects[e] > 0);
    }

    public int GetStatusEffectStacks(StatusEffectType statusEffectType)
    {
        if (_statusEffects.ContainsKey(statusEffectType)) return _statusEffects[statusEffectType];
        return 0;
    }

    public IEnumerator WaitForTweensComplete(){

        List<Tween> tweens = DOTween.TweensByTarget(transform, true);
        if(tweens != null)
            foreach (Tween t in tweens)
                yield return t.WaitForCompletion();
    }



    public void SetHealth(int health)
    {
        CurrentHealth = health;
        OnHealthChanged?.Invoke(CurrentHealth);
        UpdateHealthText();
    }
    
    public void SetMaxHealth(int maxHealth)
    {
        MaxHealth = maxHealth;
        OnMaxHealthChanged?.Invoke(MaxHealth);
        UpdateHealthText();
    }

    public bool IsValid(EffectContext context, List<CombatantFilter> filters)
    {
        return !filters.Any(f => !f.TestTarget(context, this));
    }

    public void SetTargetPreview(Color color)
    {
        if (_targetPreviewSR.color == Color.clear)
        {
            _targetPreviewSR.color = color;
        }
    }

    public void HideTargetPreview()
    {
        _targetPreviewSR.color = Color.clear;
    }

    public abstract void Die();
}
