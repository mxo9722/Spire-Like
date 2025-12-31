using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPCView : CombatantView
{
    [SerializeField] private SpriteRenderer _actionSymbol;
    [SerializeField] private SpriteRenderer _targetSymbol;
    [SerializeField] private TMP_Text _behaviorValue;
    [SerializeField] private TMP_Text _behaviorName;

    public NPCData Data { get; private set; }
    public NPCAction CurrentAction { get; private set; } = null;
    public List<NPCAction> PreviousActions { get; private set; } = new();

    public bool IsEvil { get; private set; } = false;

    private bool _highlighted = false;

    public void Setup(NPCData enemyData, SlotView slot, bool isEvil = true)
    {
        Data = enemyData;
        IsEvil = isEvil;
        UpdateBehaviorIndicator();
        SetupBase(enemyData.Health, enemyData.Image, slot);

        if (enemyData.StatusEffects.Count > 0) 
        {
            if (ActionSystem.Instance.IsPerforming)
            {
                foreach (KeyValuePair<StatusEffectType, int> se in enemyData.StatusEffects) 
                {
                    AddStatusEffectGA addStatusEffectGA = new(se.Key,se.Value,new() { this });
                    ActionSystem.Instance.AddReaction(addStatusEffectGA);
                }
            }
            else
            {
                foreach (KeyValuePair<StatusEffectType, int> se in enemyData.StatusEffects)
                {
                    AddStatusEffect(se.Key, se.Value);
                }
            }
        }

    }

    public override void OnMouseEnter()
    {
        base.OnMouseEnter();

        if (!TargetPreviewSystem.Instance.HighLighted && CurrentAction != null)
        {
            TargetPreviewSystem.Instance.SetTargetPreviews(this,CurrentAction);
            _highlighted = true;
        }
    }

    public override void OnMouseExit()
    {
        base.OnMouseExit();

        if (_highlighted)
        {
            TargetPreviewSystem.Instance.HideTargetPreviews();
            _highlighted = false;
        }
    }

    public void SetCurrentAction(NPCAction enemyAction)
    {
        if (CurrentAction != null)
            PreviousActions.Add(CurrentAction);
        CurrentAction = enemyAction;

        UpdateBehaviorIndicator();
    }

    public void UpdateBehaviorIndicator()
    {
        _behaviorValue.text = "";
        _targetSymbol.sprite = EnemySystem.Instance.GetEnemyTargetSymbol(NPCTargetTypes.NONE);

        if (CurrentAction == null)
        {
            _actionSymbol.sprite = EnemySystem.Instance.GetEnemyActionSymbol(NPCActionType.NONE);

            return;
        }

        _actionSymbol.sprite = EnemySystem.Instance.GetEnemyActionSymbol(CurrentAction.Symbol);

        EffectContext context = new(this);

        foreach (AutoTargetEffect effect in CurrentAction.Effects)
        {
            GameAction pe = effect.GetGameAction(context);

            if(pe is PerformEffectsGA performEffectsGA)
            {
                GameAction ga = performEffectsGA.GetGameAction(context);

                if(ga is SaveDataGA saveDataGA)
                    saveDataGA.SimulatedPerform();
            }


            if (effect is AutoCombatantTargetEffect combatantTargetEffect)
            {
                NPCTargetTypes intent = combatantTargetEffect.TargetMode.GetTargetIntent();

                if(intent != NPCTargetTypes.NONE)
                    _targetSymbol.sprite = EnemySystem.Instance.GetEnemyTargetSymbol(intent);


                if (effect.Effect is AttackHeroEffect attackHeroEffect)
                {
                    List<CombatantView> targets = combatantTargetEffect.TargetMode.GetTargets(context);

                    string attackText = DamageSystem.EnemyDamageTextFromAttack(attackHeroEffect.Damage, this, targets);

                    _behaviorValue.text = attackText;

                    break;
                }
                else if (effect.Effect is MultiAttackFoeEffect multiAttackFoeEffect)
                {
                    List<CombatantView> targets = combatantTargetEffect.TargetMode.GetTargets(context);

                    string attackText = DamageSystem.EnemyDamageTextFromAttack(multiAttackFoeEffect.Damage.GetAmount(context), this, targets);

                    _behaviorValue.text = multiAttackFoeEffect.AttackCount.GetAmount(context) + "x" + attackText;

                    break;
                }
            }
        }
    }

    protected override void LoadHelpBoxes(HelpBoxesUI helpBoxesUI)
    {
        if (CurrentAction != null)
        {
            NPCActionType action = CurrentAction.Symbol;

            string helpBoxText = "Enemy intends to";

            helpBoxText = helpBoxText + ' ' + EnemySystem.Instance.GetEnemyActionDescription(action);

            if (CurrentAction.Effects.Count > 0 && CurrentAction.Effects[0] is AutoCombatantTargetEffect combatantTargetEffect)
            {
                string enemyTargetDescription = EnemySystem.Instance.GetEnemyTargetDescription(combatantTargetEffect.TargetMode.GetTargetIntent());

                if(!string.IsNullOrEmpty(enemyTargetDescription))
                    helpBoxText = helpBoxText + ' ' + enemyTargetDescription;

                if (combatantTargetEffect.Effect is AttackHeroEffect attackHeroEffect)
                {
                    EffectContext targetContext = new(this);

                    List<CombatantView> targets = combatantTargetEffect.TargetMode.GetTargets(targetContext);
                    helpBoxText = helpBoxText + " for " + DamageSystem.GetDamageFromAttack(attackHeroEffect.Damage, this, targets).ToString() + " damage";
                }
            }

            helpBoxText = helpBoxText + ".";

            helpBoxesUI.AddHelpBoxFromText(action.ToString().Replace('_', ' ').ToUpper(), helpBoxText);
        }
        base.LoadHelpBoxes(helpBoxesUI);
    }

    public override void Die()
    {
        KillNpcGA killEnemyGA = new(this);
        ActionSystem.Instance.AddReaction(killEnemyGA);
    }

    public IEnumerator ApplyBehaviorText(string behaviorName, float duration)
    {
        if (CurrentHealth == 0)
            yield break;

        _behaviorName.text = behaviorName;
        Tween tween = _behaviorName.transform.DOLocalJump(Vector3.zero, 1f, 1, duration);
        yield return new WaitForSeconds(duration / 2.0f);
        _behaviorName.DOFade(0, duration / 2.0f);
        yield return tween.WaitForCompletion();
        _behaviorName.alpha = 1f;
        _behaviorName.text = "";
    }
}