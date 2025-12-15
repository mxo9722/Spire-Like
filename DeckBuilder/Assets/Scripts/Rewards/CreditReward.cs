using UnityEngine;

public class CreditReward : SetReward
{

    [SerializeField, Min(1)] private int _amount;
    public override Sprite RewardImage => RewardSystem.Instance.CreditSprite;

    public override string RewardName => _amount.ToString()+" Credits";

    public void Setcredits(int amount)
    {
        _amount = amount;
    }

    public override void CollectReward()
    {
        CreditSystem.Instance.Add(_amount);

        RewardSystem.Instance.RemoveReward(this);
    }
}
