using System.Collections.Generic;
using UnityEngine;

public class AddCardsToHandGA : GameAction
{
    public Dictionary<Card, int> Cards { get; private set; }

    public AddCardsToHandGA(List<Card> cards)
    {
        Cards = new();
        foreach (var card in cards)
        {
            Cards.Add(card, -1);
        }
    }

    public AddCardsToHandGA(Card card, int index)
    {
        Cards = new();
        Cards.Add(card, index);
    }
    
    public AddCardsToHandGA(Dictionary<Card,int> cards)
    {
        Cards = cards;
    }
}
