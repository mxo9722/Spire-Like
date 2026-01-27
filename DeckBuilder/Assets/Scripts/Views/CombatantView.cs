using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Linq;
using UnityEngine;
using System;

public abstract class CombatantView : MonoBehaviour, ITargetPreviewable, IHoldData, IComparable
{
    [SerializeField] private TMP_Text _healthText;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private StatusEffectsUI _statusEffectsUI;
    [SerializeField] private HelpBoxesUI _helpBoxesUI;

    public int MaxHealth { get; private set; }
    public int CurrentHealth { get; private set; }

    public SlotView Slot { get; private set; } = null;
    public LaneView Lane => Slot.Lane;

    public bool TargetPreviewActive => Slot.TargetPreviewActive;

    private Dictionary<StatusEffect, int> _statusEffects = new();

    public Action<int> OnHealthChanged;
    public Action<int> OnMaxHealthChanged;

    public bool MovedThisRound { get; private set; } = false;


    private Dictionary<string, object> _data = null;

    protected void SetupBase(int health, Sprite sprite, SlotView slotView)
    {
        slotView.AddCombatant(this);
        transform.localScale = Vector3.one;

        MaxHealth = CurrentHealth = health;

        Vector2 size = _spriteRenderer.size;
        _spriteRenderer.sprite = sprite;
        _spriteRenderer.size = size;
        _statusEffectsUI.SetUp(this);

        UpdateHealthText();
    }

    public void OnEnable()
    {
        ActionSystem.SubscribeReaction<BeforePlayerTurnGA>(this, BeforePlayerTurn, ReactionTiming.PRE);
    }

    private void OnDisable()
    {
        ActionSystem.UnsubscribeReaction<BeforePlayerTurnGA>(this, BeforePlayerTurn, ReactionTiming.PRE);

        transform.DOKill();
        _spriteRenderer.DOKill();
        HideTargetPreview();
    }

    public virtual void OnMouseEnter()
    {
        if (!ManualTargetSystem.Instance.IsTargetting && !CardCollectionSystem.Instance.Opened)
            LoadHelpBoxes(_helpBoxesUI);
    }

    public virtual void OnMouseExit()
    {
        _helpBoxesUI.Hide();
    }

    protected virtual void LoadHelpBoxes(HelpBoxesUI helpBoxesUI)
    {
        List<StatusEffect> allStatusEffects = GetAllActiveStatusEffects();

        foreach (StatusEffect statusEffect in allStatusEffects)
        {
            _helpBoxesUI.AddHelpBoxFromStatusEffect(statusEffect, _statusEffects[statusEffect]);
        }
    }

    private void UpdateHealthText()
    {
        _healthText.text = "HP: " + CurrentHealth;
    }

    public (int UnblockedDamage, int Overkill) Damage(int damageAmount, bool ignoreBlock = false)
    {
        int remainingDamage = damageAmount;
        int currentArmor = GetStatusEffectStacks(StatusEffect.BLOCK);

        if (remainingDamage == 0 || IsInvincible())
            return (0, 0);

        if (!ignoreBlock)
        {
            if (currentArmor >= remainingDamage)
            {
                RemoveStatusEffect(StatusEffect.BLOCK, remainingDamage);
                remainingDamage = 0;
            }
            else if (currentArmor > 0)
            {
                RemoveStatusEffect(StatusEffect.BLOCK, currentArmor);
                remainingDamage -= currentArmor;
            }
        }

        int overkill = Math.Max(remainingDamage - CurrentHealth, 0);

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

        return (remainingDamage, overkill);
    }

    public void Heal(int amount)
    {
        int newHealth = Math.Min(MaxHealth, CurrentHealth + amount); ;

        if (newHealth != CurrentHealth)
        {
            CurrentHealth = newHealth;
            OnHealthChanged?.Invoke(CurrentHealth);
            UpdateHealthText();
        }
    }
    
    public bool IsSelectable()
    {
        if (GetStatusEffectStacks(StatusEffect.STEALTH) > 0)
        {
            if (Lane.GetFriendlyCombatants(this).Where(c => c.GetStatusEffectStacks(StatusEffect.STEALTH) == 0).Count() > 0)
                return false;
        }

        return true;
    }

    public bool IsInvincible()
    {
        

        if (GetStatusEffectStacks(StatusEffect.TAUNT) == 0)
        {
            if (Lane.GetFriendlyCombatants(this).Where(c => c.GetStatusEffectStacks(StatusEffect.TAUNT) > 0).Count() > 0)
                return true;
        }

        return false;
    }

    public void AddStatusEffect(StatusEffect type, int stackCount)
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

    public void RemoveStatusEffect(StatusEffect type, int stackCount)
    {
        if (_statusEffects.ContainsKey(type))
        {
            _statusEffects[type] -= stackCount;
            if (_statusEffects[type] < 0)
                _statusEffects[type] = 0;
        }

        if (CurrentHealth > 0)
            _statusEffectsUI.UpdateStatusEffectsUI(type, _statusEffects[type]);
    }

    public List<StatusEffect> GetAllActiveStatusEffects()
    {
        List<StatusEffect> allTypes = new(_statusEffects.Keys);

        return allTypes.FindAll(e => _statusEffects[e] > 0);
    }

    public int GetStatusEffectStacks(StatusEffect statusEffectType)
    {
        if (_statusEffects.ContainsKey(statusEffectType)) return _statusEffects[statusEffectType];
        return 0;
    }

    public IEnumerator WaitForTweensComplete()
    {
        if (CurrentHealth == 0)
            yield break;

        List<Tween> tweens = DOTween.TweensByTarget(transform, true);
        if (tweens != null)
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
        Slot.SetTargetPreview(color);
    }

    public void HideTargetPreview()
    {
        Slot.HideTargetPreview();
    }

    public void SetImageAlpha(float alpha, float duration)
    {
        if (_spriteRenderer.color.a == alpha)
            return;
        _spriteRenderer.DOKill();
        _spriteRenderer.DOFade(alpha, duration);
    }

    public void SetSlot(SlotView slotView)
    {
        Slot = slotView;
    }

    public virtual int GetSortValue()
    {
        if (GetStatusEffectStacks(StatusEffect.STEALTH) > 0)
            return 1;

        if (GetStatusEffectStacks(StatusEffect.GUARD) > 0)
            return -1;
        
        if (GetStatusEffectStacks(StatusEffect.TAUNT) > 0)
            return -2;

        return 0;
    }

    private void BeforePlayerTurn(BeforePlayerTurnGA beforePlayerTurnGA)
    {
        MovedThisRound = false;
    }

    public void SetMoved(bool moved)
    {
        MovedThisRound = true;
    }

    public void AddData(string key, object data)
    {
        if (_data == null)
            _data = new();

        if (_data.ContainsKey(key))
            _data[key] = data;
        else
            _data.Add(key, data);
    }

    public T GetData<T>(string key)
    {
        if (_data == null || !_data.ContainsKey(key))
            return default(T);

        if (_data[key] is T t)
            return t;

        return default(T);
    }

    public bool ContainsKey(string key)
    {
        if (_data == null)
            return false;

        return _data.ContainsKey(key);
    }

    public abstract void Die();

    public int CompareTo(object obj)
    {
        CombatantView cv = (CombatantView)obj;

        return cv.GetSortValue() - GetSortValue();
    }
}
