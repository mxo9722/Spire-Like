using UnityEngine;

public class RandomCardReward : Reward
{
    private CardReward _reward;

    public override SetReward GetSetReward()
    {
        return _reward;
    }

    public override void SetUp()
    {
        _reward = RewardCreator.Instance.CreateCardPick();
    }
}
