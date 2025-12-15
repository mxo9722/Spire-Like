using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardCollectionSystem : Singleton<CardCollectionSystem>
{
    [SerializeField] private CardCollectionUI _cardCollectionUI;

    public List<Card> Cards { get; private set; } = null;

    public bool Opened => _cardCollectionUI.Opened;
    public bool WaitingForSelection => _cardCollectionUI.WaitingForSelection;

    public void Display(List<CardData> cardCollection, bool hideOrder)
    {
        Display(cardCollection.Select(c => new Card(c)).ToList(), hideOrder);
    }

    public void Display(List<Card> cardCollection, bool hideOrder)
    {
        if (Opened)
        {
            Close();
        }

        Cards = cardCollection;

        if (hideOrder)
        {
            cardCollection = new(cardCollection);
            cardCollection.Sort((a, b) => string.Compare(a.Title, b.Title));
        }

        _cardCollectionUI.SetUp(cardCollection);
    }

    public void SelectionDisplay(List<Card> cardCollection, int amount, bool hideOrder)
    {
        if (Opened)
        {
            Close();
        }

        Cards = cardCollection;

        if (hideOrder)
        {
            cardCollection = new(cardCollection);
            cardCollection.Sort((a, b) => string.Compare(a.Title, b.Title));
        }

        _cardCollectionUI.SetUpSelection(cardCollection, amount);
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

    public List<Card> GetCardSelections() => _cardCollectionUI.GetCardSelections();
}
