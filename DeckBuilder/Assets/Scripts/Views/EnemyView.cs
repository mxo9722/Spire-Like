using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyView : CombatantView
{
    [SerializeField] private SpriteRenderer _actionSymbol;
    [SerializeField] private TMP_Text _attackText;

    public EnemyData Data { get; private set; }
    public EnemyAction CurrentAction { get; private set; } = null;
    public List<EnemyAction> PreviousActions { get; private set; } = new();

    public void Setup(EnemyData enemyData)
    {
        Data = enemyData;
        UpdateBehaviorIndicator();
        SetupBase(enemyData.Health, enemyData.Image);
    }

    public void SetCurrentAction(EnemyAction enemyAction)
    {
        if (CurrentAction != null)
            PreviousActions.Add(CurrentAction);
        CurrentAction = enemyAction;

        UpdateBehaviorIndicator();
    }

    private void UpdateBehaviorIndicator()
    {
        if (CurrentAction == null)
            return;

        _actionSymbol.sprite = EnemySystem.Instance.GetEnemyActionSymbol(CurrentAction.Symbol);
        _attackText.text = "";
        if(CurrentAction.Effects[0].Effect is AttackHeroEffect attackHeroEffect)
        {
            _attackText.text = attackHeroEffect.Damage.ToString();
        }
    }

    public override void Die()
    {
        KillEnemyGA killEnemyGA = new(this);
        ActionSystem.Instance.AddReaction(killEnemyGA);
    }
}