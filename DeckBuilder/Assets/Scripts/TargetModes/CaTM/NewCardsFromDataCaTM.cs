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
            cards.Add(new(data));
        }

        return cards;
    }
}
