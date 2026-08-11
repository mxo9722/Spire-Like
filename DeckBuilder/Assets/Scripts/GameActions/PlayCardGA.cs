using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayCardGA : GameAction
{
    public Card card { get; set; }
    public CombatantView ManualEnemyTarget { get; private set; } = null;
    public LaneView ManualLaneTarget { get; private set; } = null;
    public bool PayCost { get; private set; } = true;

    private int _playedHandIndex = -1;
    private int _playedHandSize = -1;

    public PlayCardGA(Card card, bool payCost = true)
    {
        this.card = card;
        ManualEnemyTarget = null;
        PayCost = payCost;
        SetUp();
    }
    
    public PlayCardGA(Card card, CombatantView manualTarget, bool payCost = true)
    {
        this.card = card;
        ManualEnemyTarget = manualTarget;
        PayCost = payCost;
        SetUp();
    }
    
    public PlayCardGA(Card card, LaneView manualTarget, bool payCost = true)
    {
        this.card = card;
        ManualLaneTarget = manualTarget;
        PayCost = payCost;
        SetUp();
    }

    private void SetUp()
    {
        List<Card> hand = CardSystem.Instance.GetHand();
        if (hand.Contains(card))
        {
            _playedHandIndex = hand.IndexOf(card);
            _playedHandSize = hand.Count;
        }
    }

    public EffectContext GetEffectContext()
    {
        EffectContext context = new EffectContext(null, ManualLaneTarget, ManualEnemyTarget, card, _playedHandIndex, _playedHandSize);

        context.SetCaster(card.GetOwnerView(context));

        return context;
    }
}
