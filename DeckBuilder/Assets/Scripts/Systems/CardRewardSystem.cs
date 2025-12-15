using System.Collections.Generic;
using UnityEngine;

public class CardRewardSystem : Singleton<CardRewardSystem>
{
    [SerializeField] private CardRewardView _cardChoiceView;

    private CardReward _cardReward;

    public void Show(CardReward cardReward)
    {
        _cardReward = cardReward;
        _cardChoiceView.SetUp(cardReward, ChooseCard);
    }

    private void ChooseCard(CardView cardView)
    {
        if (cardView != null)
        {
            RunSystem.Instance.AddCard(cardView.Card);

            RewardSystem.Instance.RemoveReward(_cardReward);
        }

        _cardChoiceView.Hide();
        RewardSystem.Instance.Show();
        RunSystem.Instance.SaveRun();
    }
}
