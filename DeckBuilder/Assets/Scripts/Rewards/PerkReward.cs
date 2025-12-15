using UnityEngine;

[System.Serializable]
public class PerkReward : SetReward
{
    [SerializeField] private PerkData _perkData;

    public override Sprite RewardImage => _perkData.Image;

    public override string RewardName => _perkData.name;

    public void SetPerk(PerkData perkData)
    {
        _perkData = perkData;
    }

    public override void CollectReward()
    {
        PerkSystem.Instance.ObtainPerk(_perkData);

        RewardSystem.Instance.RemoveReward(this);
    }

}
