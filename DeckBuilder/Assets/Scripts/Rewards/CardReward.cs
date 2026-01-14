using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardReward : SetReward
{
    public override Sprite RewardImage => RewardSystem.Instance.CardSprite;

    public override string RewardName => "Take a card!";

    [field: SerializeField] public List<CardData> Cards { get; private set; }

    public void SetCards(CardData card)
    {
        Cards = new() { card };
    }
    
    public void SetCards(List<CardData> cards)
    {
        Cards = cards;
    }

    public override void CollectReward()
    {
        if (Cards.Count > 1)
        {
            CardRewardSystem.Instance.Show(this);

            RewardSystem.Instance.Hide();
        }
        else if(Cards.Count == 1)
        {
            RunSystem.Instance.AddCard(new(Cards[0]));
        }
    }
}
