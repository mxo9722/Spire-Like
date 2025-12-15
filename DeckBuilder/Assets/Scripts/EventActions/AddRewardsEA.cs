using AYellowpaper.SerializedCollections;
using SerializeReferenceEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XNode;

[Serializable]
public class AddRewardsEA : EventAction
{

    [field: SerializeReference, SR] public List<Reward> Rewards { get; private set; }

    private bool _rewardsCollected = false;

    public override IEnumerator Invoke()
    {
        _rewardsCollected = false;

        List<SetReward> rewards;

        Rewards.ForEach(r => r.SetUp());
        rewards = Rewards.Select(e => e.GetSetReward()).ToList();

        RewardSystem.Instance.DisplayRewards(rewards, RewardsCollected);
        yield return new WaitUntil(() => _rewardsCollected);
    }

    private void RewardsCollected()
    {
        _rewardsCollected = true;
    }
}
