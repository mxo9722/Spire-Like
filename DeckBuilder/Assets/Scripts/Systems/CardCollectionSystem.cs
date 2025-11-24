using System;
using System.Collections.Generic;
using UnityEngine;

public class CardCollectionSystem : PersistentSingleton<CardCollectionSystem>
{
    private CardCollectionUI _cardCollectionUI => CardCollectionUI.Instance;

    public List<Card> Cards { get; private set; } = null;

    public bool Opened => _cardCollectionUI.Opened;

    public void Display(List<Card> cardCollection, bool hideOrder)
    {
        if (Opened)
        {
            Close();
        }

        Cards = cardCollection;

        if (hideOrder)
        {
            List<Card> sortedCardCollection = new(cardCollection);
            sortedCardCollection.Sort((a, b) => string.Compare(a.Title, b.Title));
            _cardCollectionUI.SetUp(sortedCardCollection);
        }
        else
        {
            _cardCollectionUI.SetUp(cardCollection);
        }


        foreach (CardUI cardUI in _cardCollectionUI.CardUIs)
        {
            cardUI.OnClicked.AddListener(() => { OnCardClicked(cardUI.Card); });
        }
    }

    public void Close()
    {
        Cards = null;

        foreach (CardUI cardUI in _cardCollectionUI.CardUIs)
        {
            cardUI.OnClicked.RemoveAllListeners();
        }

        _cardCollectionUI.Close();
    }

    private void OnCardClicked(Card card)
    {
        //TODO: add some stuff here
    }
}
