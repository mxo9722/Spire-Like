using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ShopView : MonoBehaviour
{

    [SerializeField] private List<ShopCardView> _shopCardViews;
    [SerializeField] private SpriteRenderer _shopKeeper;
    [SerializeField] private Transform _shopKeeperSpeechTrans;

    [SerializeField, TextArea(2,4)] private string _shopKeeperDialogue;

    [SerializeField, SerializedDictionary("Rarity", "Cost Range")] private SerializedDictionary<Rarity,Vector2Int> _cardPrices;

    public void SetUp()
    {
        CardData[] cards = RewardCreator.Instance.GetRandomCards(_shopCardViews.Count);

        int saleIndex = RNG.Random.Next(_shopCardViews.Count);

        for(int i=0;i<_shopCardViews.Count;i++)
        {
            CardData cardData = cards[i];

            Vector2Int priceRange = _cardPrices[cardData.Rarity];
            int price = RNG.Random.Next(priceRange.x, priceRange.y+1);

            if (i == saleIndex)
                price /= 2;

            ShopCardView shopCard = _shopCardViews[i];
            shopCard.SetUp(new(cardData), price, _shopKeeper.transform.position);
            
            if (i == saleIndex)
                shopCard.AddSaleBanner();

            shopCard.OnButtonPressed += OnCardPressed;
        }

        SpeechBubbleUI speechBubble = SpeechBubbleSystem.Instance.DisplaySpeechBubble(_shopKeeperSpeechTrans.position + Vector3.up, _shopKeeperDialogue, Vector3.one * 3);
        StartCoroutine(speechBubble.PlayWordBubble(10));
    }

    private void OnCardPressed(ShopCardView shopCardView)
    {
        if (RenownSystem.Instance.TrySpend(shopCardView.Cost))
        {
            CardReward cardReward = new();
            cardReward.SetCards(shopCardView.Card.data);
            cardReward.CollectReward();
            shopCardView.SetUp(null, 0, _shopKeeper.transform.position);
        }
    }
}
