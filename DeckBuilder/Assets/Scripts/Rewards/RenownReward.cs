using UnityEngine;

public class RenownReward : SetReward
{

    [SerializeField, Min(1)] private int _amount;
    public override Sprite RewardImage => RewardSystem.Instance.CreditSprite;

    public override string RewardName => _amount.ToString()+" Renown";

    public void Setcredits(int amount)
    {
        _amount = amount;
    }

    public override void CollectReward()
    {
        RenownSystem.Instance.Add(_amount);

        RewardSystem.Instance.RemoveReward(this);
    }
}
