using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Linq;
using UnityEngine;
using System;
using UnityEngine.UI;

public abstract class CombatantView : MonoBehaviour, ITargetPreviewable, IHoldData, IComparable
{
    [SerializeField] private Image _healthBox;
    [SerializeField] private TMP_Text _healthText;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private StatusEffectsUI _statusEffectsUI;
    [SerializeField] private HelpBoxesUI _helpBoxesUI;
    [SerializeField] private Transform _blockPosition;

    public int MaxHealth { get; private set; }
    public int CurrentHealth { get; private set; }

    public SlotView Slot { get; private set; } = null;
    public LaneView Lane => Slot.Lane;

    public bool TargetPreviewActive => Slot.TargetPreviewActive;

    private Dictionary<StatusEffectInfo, int> _statusEffects = new(new SEEqualityComparer());

    public Action<int> OnHealthChanged;
    public Action<int> OnMaxHealthChanged;

    public bool MovedThisRound { get; private set; } = false;

    private Dictionary<string, object> _data = null;

    private bool _isDragged = false;

    protected void SetupBase(int health, Sprite sprite, SlotView slotView)
    {
        slotView.AddCombatant(this);
        transform.localScale = Vector3.one;

        MaxHealth = CurrentHealth = health;

        Vector2 size = _spriteRenderer.size;
        _spriteRenderer.sprite = sprite;
        _spriteRenderer.size = size;

        _statusEffectsUI.SetUp(this, _blockPosition);

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

    private void OnMouseDown()
    {
        if (!DragUnitSystem.Instance.CanDragUnits) return;
        if (DragUnitSystem.Instance.CombatantFilters.Any(f => !f.TestTarget(DragUnitSystem.Instance.EffectContext, this))) return;
        if (GetStatusEffectStacks(StatusEffect.PINNED) > 0) return;
        
        transform.DOKill(true);
        _isDragged = true;

        DragUnitSystem.Instance.HighlightValidLanes(this);
    }

    private void OnMouseDrag()
    {
        if (!_isDragged) return;
        transform.position = MouseUtil.GetMousePositionInWorldSpace(transform.position.z);
    }

    private void OnMouseUp()
    {
        if (!_isDragged) return;

        LaneView laneView = ManualTargetSystem.Instance.EndLaneTargeting(MouseUtil.GetMousePositionInWorldSpace(-1));

        bool laneIsValid = DragUnitSystem.Instance.LaneFilters.TrueForAll(f => f.TestTarget(DragUnitSystem.Instance.EffectContext, laneView));

        if (laneView != null && laneView.HeroView != this && laneIsValid)
        {
            StartCoroutine(DragUnitSystem.Instance.EndDrag(this, laneView));
        }
        else
        {
            transform.DOLocalMove(Vector3.zero, 0.3f);
            DragUnitSystem.Instance.HighlightValidUnits();
        }

        _isDragged = false;
    }

    protected virtual void LoadHelpBoxes(HelpBoxesUI helpBoxesUI)
    {
        List<StatusEffectInfo> allStatusEffects = GetAllActiveStatusEffects();

        foreach (StatusEffectInfo statusEffect in allStatusEffects)
        {
            _helpBoxesUI.AddHelpBoxFromStatusEffect(statusEffect, _statusEffects[statusEffect]);
        }
    }

    private void UpdateHealthText()
    {
        float fillAmount = (float)CurrentHealth / (float)MaxHealth;
        _healthBox.fillAmount = fillAmount;
        _healthBox.color = DamageSystem.Instance.HealthGradiant.Evaluate(fillAmount);
        _healthText.text = CurrentHealth.ToString() + "/" + MaxHealth.ToString();
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
                RemoveStatusEffect(StatusEffectSystem.GetDictionaryEntry(StatusEffect.BLOCK), remainingDamage);
                remainingDamage = 0;
            }
            else if (currentArmor > 0)
            {
                RemoveStatusEffect(StatusEffectSystem.GetDictionaryEntry(StatusEffect.BLOCK), currentArmor);
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

    public void AddStatusEffect(StatusEffectInfo info, int stackCount)
    {
        if (_statusEffects.ContainsKey(info))
        {
            _statusEffects[info] += stackCount;
        }
        else
        {
            _statusEffects.Add(info, stackCount);
        }

        _statusEffectsUI.UpdateStatusEffectsUI(info, _statusEffects[info]);
    }

    public void RemoveStatusEffect(StatusEffectInfo info, int stackCount)
    {
        if (_statusEffects.ContainsKey(info))
        {
            _statusEffects[info] -= stackCount;
            if (_statusEffects[info] < 0)
                _statusEffects[info] = 0;
        }

        if (CurrentHealth > 0)
            _statusEffectsUI.UpdateStatusEffectsUI(info, _statusEffects[info]);
    }

    public List<StatusEffectInfo> GetAllActiveStatusEffects()
    {
        List<StatusEffectInfo> allTypes = new(_statusEffects.Keys);

        return allTypes.FindAll(e => _statusEffects[e] > 0);
    }

    public int GetStatusEffectStacks(StatusEffect statusEffectType)
    {
        StatusEffectInfo info = StatusEffectSystem.GetDictionaryEntry(statusEffectType);

        if (_statusEffects.ContainsKey(info)) return _statusEffects[info];
        return 0;
    }
    
    public int GetStatusEffectStacks(StatusEffectInfo statusEffectInfo)
    {
        if (_statusEffects.ContainsKey(statusEffectInfo)) return _statusEffects[statusEffectInfo];
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

    public void SetData(string key, object data)
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

    public int CompareTo(object obj)
    {
        CombatantView cv = (CombatantView) obj;

        return cv.GetSortValue() - GetSortValue();
    }

    public int GetLaneDistance(CombatantView compare)
    {
        return BoardSystem.Instance.GetLaneDistance(Lane, compare.Lane);
    }

    public abstract void Die();
}
