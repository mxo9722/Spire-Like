using System.Collections.Generic;
using UnityEngine;

public class CardCollectionUI : Singleton<CardCollectionUI>
{
    [SerializeField] private Transform _contentTransform;
    [SerializeField] private GameObject _wrapper;

    [SerializeField] private CardUI _cardUIPrefab;

    public bool Opened { get => _wrapper.activeSelf; }

    public List<CardUI> CardUIs { get; private set; } = new();

    public void SetUp(List<Card> cardCollection)
    {
        _wrapper.SetActive(true);

        foreach (Card card in cardCollection)
        {
            CardUI cardUI = Instantiate(_cardUIPrefab, _contentTransform);
            cardUI.SetUp(card);
            CardUIs.Add(cardUI);
        }
    }

    public void Close()
    {
        foreach(CardUI cardUI in CardUIs)
        {
            Destroy(cardUI.gameObject);
        }

        CardUIs.Clear();
        _wrapper.SetActive(false);
    }
}
