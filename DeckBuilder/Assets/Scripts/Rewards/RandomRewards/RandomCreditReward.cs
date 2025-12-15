using UnityEngine;

public class RandomCreditReward : Reward
{
    [SerializeField] private int _min = 10;
    [SerializeField] private int _max = 20;


    private CreditReward _setReward;


    public override SetReward GetSetReward()
    {
        return _setReward;
    }

    public override void SetUp()
    {
        _setReward = RewardCreator.Instance.CreateMoney(_min, _max);
    }
}
