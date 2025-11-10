using UnityEngine;

public class MoveEnemyGA : GameAction
{
    public LaneView DestinationLane { get; private set; }
    public EnemyView EnemyView { get; private set; }

    public MoveEnemyGA(LaneView destinationLane,EnemyView enemyView)
    {
        DestinationLane = destinationLane;
        EnemyView = enemyView;
    }
}
