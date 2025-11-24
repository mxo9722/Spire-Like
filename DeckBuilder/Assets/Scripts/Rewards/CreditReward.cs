using UnityEngine;

public class CreditReward : Reward
{

    [SerializeField, Min(1)] private int _amount;
    public override Sprite RewardImage => RewardSystem.Instance.CreditSprite;

    public override string RewardName => _amount.ToString()+" Credits";

    public override void CollectReward()
    {
        RunSystem.Instance.AddCredit(_amount);
    }
}
