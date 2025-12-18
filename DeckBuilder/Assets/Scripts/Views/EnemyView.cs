using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyView : CombatantView
{
    [SerializeField] private SpriteRenderer _actionSymbol;
    [SerializeField] private SpriteRenderer _targetSymbol;
    [SerializeField] private TMP_Text _attackText;

    public EnemyData Data { get; private set; }
    public EnemyAction CurrentAction { get; private set; } = null;
    public List<EnemyAction> PreviousActions { get; private set; } = new();

    public void Setup(EnemyData enemyData, SlotView slot)
    {
        Data = enemyData;
        UpdateBehaviorIndicator();
        SetupBase(enemyData.Health, enemyData.Image, slot);
    }

    public void SetCurrentAction(EnemyAction enemyAction)
    {
        if (CurrentAction != null)
            PreviousActions.Add(CurrentAction);
        CurrentAction = enemyAction;

        UpdateBehaviorIndicator();
    }

    public void UpdateBehaviorIndicator()
    {
        _attackText.text = "";
        _targetSymbol.sprite = EnemySystem.Instance.GetEnemyTargetSymbol(EnemyTargetTypes.NONE);

        if (CurrentAction == null)
        {
            _actionSymbol.sprite = EnemySystem.Instance.GetEnemyActionSymbol(EnemyActionType.NONE);

            return;
        }

        _actionSymbol.sprite = EnemySystem.Instance.GetEnemyActionSymbol(CurrentAction.Symbol);

        if (CurrentAction.Effects[0] is AutoCombatantTargetEffect combatantTargetEffect)
        {
            _targetSymbol.sprite = EnemySystem.Instance.GetEnemyTargetSymbol(combatantTargetEffect.TargetMode.GetTargetIntent());

            if (CurrentAction.Effects[0].Effect is AttackHeroEffect attackHeroEffect)
            {
                EffectContext targetModeContext = new(this);

                List<CombatantView> targets = combatantTargetEffect.TargetMode.GetTargets(targetModeContext);

                AttackHeroGA attackGameAction = (AttackHeroGA)attackHeroEffect.GetGameAction(new(this), targets);

                string attackText = DamageSystem.EnemyDamageTextFromAttack(attackGameAction.Damage, this, targets);

                _attackText.text = attackText;
            }
        }
    }

    protected override void LoadHelpBoxes(HelpBoxesUI helpBoxesUI)
    {
        if (CurrentAction != null)
        {
            EnemyActionType action = CurrentAction.Symbol;

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
        KillEnemyGA killEnemyGA = new(this);
        ActionSystem.Instance.AddReaction(killEnemyGA);
    }
}