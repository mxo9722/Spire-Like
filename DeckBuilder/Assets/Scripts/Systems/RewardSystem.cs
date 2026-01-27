using System;
using System.Collections.Generic;
using UnityEngine;

public class RewardSystem : Singleton<RewardSystem>
{
    [SerializeField] private RewardsUI _rewardsUI;
    [field: SerializeField] public Sprite CreditSprite { get; private set; }
    [field: SerializeField] public Sprite CardSprite { get; private set; }

    public bool RewardsUIOpened { get => _rewardsUI.IsOpen; }

    public void DisplayRewards(List<SetReward> rewards, Action onClose) => _rewardsUI.SetUp(rewards, onClose);
    public void RemoveReward(SetReward reward) => _rewardsUI.RemoveReward(reward);

    public void Hide() => _rewardsUI.Hide();
    public void Show() => _rewardsUI.Show();
}
