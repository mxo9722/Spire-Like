using UnityEngine;

public class DetermineEnemyBehaviorGA : GameAction
{
    public EnemyView EnemyView { get; private set; }

    public DetermineEnemyBehaviorGA(EnemyView enemyView)
    {
        EnemyView = enemyView;
    }
}
