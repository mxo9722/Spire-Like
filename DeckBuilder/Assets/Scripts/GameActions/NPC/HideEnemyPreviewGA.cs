using UnityEngine;

public class HideEnemyPreviewGA : GameAction
{
    public NPCView EnemyView { get;  private set; }

    public HideEnemyPreviewGA(NPCView enemyView)
    {
        EnemyView = enemyView;
    }
}
