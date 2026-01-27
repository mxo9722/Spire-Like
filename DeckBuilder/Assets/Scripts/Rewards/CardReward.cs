using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardReward : SetReward
{
    public override Sprite RewardImage => RewardSystem.Instance.CardSprite;

    public override string RewardName => "Take a card!";

    [field: SerializeField] public List<Card> Cards { get; private set; }

    public void SetCards(Card card)
    {
        Cards = new() { card };
    }
    
    public void SetCards(List<Card> cards)
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
            RunSystem.Instance.AddCard(Cards[0]);
        }
    }
}
