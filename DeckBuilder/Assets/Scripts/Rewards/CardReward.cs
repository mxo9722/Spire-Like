using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardReward : SetReward
{
    public override Sprite RewardImage => RewardSystem.Instance.CardSprite;

    public override string RewardName => "Take a card!";

    [field: SerializeField] public List<CardData> Cards { get; private set; }

    public void SetCards(List<CardData> cards)
    {
        Cards = cards;
    }

    public override void CollectReward()
    {
        CardRewardSystem.Instance.Show(this);

        RewardSystem.Instance.Hide();
    }
}
