using UnityEngine;

public class HideEnemyPreviewGA : GameAction
{
    public EnemyView EnemyView { get;  private set; }

    public HideEnemyPreviewGA(EnemyView enemyView)
    {
        EnemyView = enemyView;
    }
}
