using TMPro;
using UnityEngine;

public class EnemyView : CombatantView
{
    [SerializeField] private TMP_Text _attackText;

    public int AttackPower { get; set; }


    public void Setup(EnemyData enemyData)
    {
        AttackPower = enemyData.AttackPower;
        UpdateAttackText();
        SetupBase(enemyData.Health, enemyData.Image);
    }

    private void UpdateAttackText()
    {
        _attackText.text = "ATK: " + AttackPower;
    }

    public override void Die()
    {
        KillEnemyGA killEnemyGA = new(this);
        ActionSystem.Instance.AddReaction(killEnemyGA);
    }
}