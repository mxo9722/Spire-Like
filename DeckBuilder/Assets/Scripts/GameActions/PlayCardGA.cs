using UnityEngine;

public class PlayCardGA : GameAction
{
    public Card card { get; set; }
    public EnemyView ManualTarget { get; private set; }

    public PlayCardGA(Card card)
    {
        this.card = card;
        ManualTarget = null;
    }
    
    public PlayCardGA(Card card, EnemyView manualTarget)
    {
        this.card = card;
        ManualTarget = manualTarget;
    }
}
