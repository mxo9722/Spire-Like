using UnityEngine;

public class RandomPerkReward : Reward
{
    private PerkReward _reward;

    public override SetReward GetSetReward()
    {
        return _reward;
    }

    public override void SetUp()
    {
        _reward = RewardCreator.Instance.CreatePerk();
    }
}
