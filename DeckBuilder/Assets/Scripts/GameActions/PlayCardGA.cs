using UnityEngine;

public class PlayCardGA : GameAction
{
    public Card card { get; set; }
    public CombatantView ManualEnemyTarget { get; private set; } = null;
    public LaneView ManualLaneTarget { get; private set; } = null;
    public bool PayCost { get; private set; } = true;

    public PlayCardGA(Card card, bool payCost = true)
    {
        this.card = card;
        ManualEnemyTarget = null;
        PayCost = payCost;
    }
    
    public PlayCardGA(Card card, CombatantView manualTarget, bool payCost = true)
    {
        this.card = card;
        ManualEnemyTarget = manualTarget;
        PayCost = payCost;
    }
    
    public PlayCardGA(Card card, LaneView manualTarget, bool payCost = true)
    {
        this.card = card;
        ManualLaneTarget = manualTarget;
        PayCost = payCost;
    }

    public EffectContext GetEffectContext()
    {

        EffectContext context = new EffectContext(null, ManualLaneTarget, ManualEnemyTarget, card);

        context.SetCaster(card.GetOwnerView(context));

        return context;
    }
}
