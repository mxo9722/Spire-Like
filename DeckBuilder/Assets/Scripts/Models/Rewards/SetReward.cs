using UnityEngine;

[System.Serializable]
public abstract class SetReward : Reward
{
    public abstract Sprite RewardImage { get; }
    public abstract string RewardName { get; }
    public virtual string RewardDescription { get => ""; }
    public virtual bool ShowTip { get => false; }

    public SetReward() { }

    public abstract void CollectReward();

    public override SetReward GetSetReward()
    {
        return this;
    }

    public override void SetUp() { }
}
