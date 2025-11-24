using UnityEngine;

[System.Serializable]
public abstract class Reward
{
    public abstract Sprite RewardImage { get; }
    public abstract string RewardName { get; }

    public abstract void CollectReward();
}
