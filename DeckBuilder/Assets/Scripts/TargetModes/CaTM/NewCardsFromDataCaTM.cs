using System.Collections.Generic;
using UnityEngine;

public class NewCardsFromDataCaTM : CardTargetMode
{
    [SerializeField] private List<CardData> _cards;

    public override List<Card> GetTargets(EffectContext context)
    {
        List<Card> cards = new();

        foreach(CardData data in _cards)
        {

            Card card = new(data);
              
            if(context.PlayedCard != null)
                card = new(data, context.PlayedCard.Owner);

            cards.Add(card);
        }

        return cards;
    }

    
}
