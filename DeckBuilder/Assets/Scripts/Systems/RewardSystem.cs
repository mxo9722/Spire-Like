using System.Collections.Generic;
using UnityEngine;

public class RewardSystem : Singleton<RewardSystem>
{
    [SerializeField] private RewardsUI _rewardsUI;
    [field: SerializeField] public Sprite CreditSprite { get; private set; }
    [field: SerializeField] public Sprite CardSprite { get; private set; }

    public void Display(List<Reward> rewards) => _rewardsUI.SetUp(rewards);
}
