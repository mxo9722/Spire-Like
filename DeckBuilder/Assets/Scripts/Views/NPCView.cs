using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPCView : CombatantView
{
    [SerializeField] private NPCActionView _actionView1;
    [SerializeField] private NPCActionView _actionView2;
    [SerializeField] private NPCTargetView _targetView;
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

        int health = enemyData.Health + RNG.Random.Next(enemyData.RandomHealthMod);

        SetupBase(health, enemyData.Image, slot);
        _targetView.SetUp(this);
        _actionView1.SetUp(this);
        _actionView2.SetUp(this);

        if (enemyData.StatusEffects.Count > 0) 
        {
            if (ActionSystem.Instance.IsPerforming)
            {
                foreach (KeyValuePair<StatusEffect, int> se in enemyData.StatusEffects) 
                {
                    AddStatusEffectGA addStatusEffectGA = new(se.Key,se.Value,new() { this });
                    ActionSystem.Instance.AddReaction(addStatusEffectGA);
                }
            }
            else
            {
                foreach (KeyValuePair<StatusEffect, int> se in enemyData.StatusEffects)
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
            TargetPreviewSystem.Instance.SetTargetPreviews(this, CurrentAction);
            _highlighted = true;
        }
    }

    public override void OnMouseExit()
    {
        base.OnMouseExit();

        if (_highlighted)
        {
            TargetPreviewSystem.Instance.HideTargetPreviews(true);
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

    public void UpdateBehaviorIndicator(NPCAction npcAction = null)
    {
        if (npcAction == null)
            npcAction = CurrentAction;

        _behaviorValue.text = "";
        _targetView.SetTargetPreview(NPCTargetTypes.NONE);

        if (npcAction == null)
        {
            _actionView1.SetActionPreview(NPCActionType.NONE);
            _actionView2.SetActionPreview(NPCActionType.NONE);

            return;
        }

        _actionView1.SetActionPreview(npcAction.Symbol);
        _actionView2.SetActionPreview(npcAction.Symbol2);

        EffectContext context = new(this);

        foreach (AutoTargetEffect effect in npcAction.Effects)
        {
            GameAction pe = effect.GetGameAction(context);

            if(pe is PerformEffectsGA performEffectsGA)
            {
                GameAction ga = performEffectsGA.GetGameAction(context);

                if(ga is SaveDataGA saveDataGA)
                    saveDataGA.SimulatedPerform();
            }

            NPCTargetTypes intent = effect.GetTargetIntent();

            if (intent != NPCTargetTypes.NONE)
                _targetView.SetTargetPreview(intent);

            if (effect is AutoCombatantTargetEffect combatantTargetEffect)
            {
                

                if (combatantTargetEffect.Effect is AttackHeroEffect attackHeroEffect)
                {
                    List<CombatantView> targets = combatantTargetEffect.TargetMode.GetTargets(context);

                    string attackText = DamageSystem.EnemyDamageTextFromAttack(attackHeroEffect.Damage, context, targets);

                    _behaviorValue.text = attackText;

                    break;
                }
                else if (combatantTargetEffect.Effect is MultiAttackFoeEffect multiAttackFoeEffect)
                {
                    List<CombatantView> targets = combatantTargetEffect.TargetMode.GetTargets(context);

                    string attackText = DamageSystem.EnemyDamageTextFromAttack(multiAttackFoeEffect.Damage.GetAmount(context), context, targets);

                    _behaviorValue.text = multiAttackFoeEffect.AttackCount.GetAmount(context) + "x" + attackText;

                    break;
                }
            }
        }
    }

    public override int GetSortValue()
    {
        int invert = IsEvil ? -1 : 1;

        return base.GetSortValue() * invert;
    }

    protected override void LoadHelpBoxes(HelpBoxesUI helpBoxesUI)
    {
        if (CurrentAction != null)
        {
            NPCActionType action = CurrentAction.Symbol;

            string helpBoxText = Data.name + " intends to";

            helpBoxText = helpBoxText + ' ' + EnemySystem.Instance.GetEnemyActionDescription(action);

            string enemyTargetDescription = EnemySystem.Instance.GetEnemyTargetDescription(CurrentAction.Effects[0].GetTargetIntent());

            if (!string.IsNullOrEmpty(enemyTargetDescription))
                helpBoxText = helpBoxText + ' ' + enemyTargetDescription;

            if (CurrentAction.Effects.Count > 0 && CurrentAction.Effects[0] is AutoCombatantTargetEffect combatantTargetEffect)
            {

                if (combatantTargetEffect.Effect is AttackHeroEffect attackHeroEffect)
                {
                    EffectContext targetContext = new(this);

                    List<CombatantView> targets = combatantTargetEffect.TargetMode.GetTargets(targetContext);
                    helpBoxText = helpBoxText + " for " + DamageSystem.GetDamageFromAttack(attackHeroEffect.Damage, targetContext, targets).ToString() + " damage";
                }
                else if (combatantTargetEffect.Effect is MultiAttackFoeEffect multiAttackFoeEffect)
                {
                    EffectContext context = new(this);

                    List<CombatantView> targets = combatantTargetEffect.TargetMode.GetTargets(context);
                    helpBoxText = helpBoxText + " for " + DamageSystem.GetDamageFromAttack(multiAttackFoeEffect.Damage.GetAmount(context), context, targets).ToString() 
                        + " damage " + multiAttackFoeEffect.AttackCount.GetAmount(context) + " times";
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