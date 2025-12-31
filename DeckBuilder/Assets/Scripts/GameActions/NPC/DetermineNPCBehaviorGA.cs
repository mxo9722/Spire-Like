using UnityEngine;

public class DetermineNPCBehaviorGA : GameAction
{
    public NPCView EnemyView { get; private set; }

    public DetermineNPCBehaviorGA(NPCView enemyView)
    {
        EnemyView = enemyView;
    }
}
