using UnityEngine;

public class PlayCardGA : GameAction
{
    public Card card { get; set; }
    public EnemyView ManualEnemyTarget { get; private set; } = null;
    public LaneView ManualLaneTarget { get; private set; } = null;


    public PlayCardGA(Card card)
    {
        this.card = card;
        ManualEnemyTarget = null;
    }
    
    public PlayCardGA(Card card, EnemyView manualTarget)
    {
        this.card = card;
        ManualEnemyTarget = manualTarget;
    }
    
    public PlayCardGA(Card card, LaneView manualTarget)
    {
        this.card = card;
        ManualLaneTarget = manualTarget;
    }
}
