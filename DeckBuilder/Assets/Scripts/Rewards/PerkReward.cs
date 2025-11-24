using UnityEngine;

[System.Serializable]
public class PerkReward : Reward
{
    [SerializeField] private PerkData _perkData;

    public override Sprite RewardImage => _perkData.Image;

    public override string RewardName => _perkData.name;

    public override void CollectReward()
    {
        RunSystem.Instance.AddPerk(_perkData);
    }
}
