using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardsUI : MonoBehaviour
{
    [SerializeField] private Transform _rewardItems;
    [SerializeField] private GameObject _wrapper;
    [SerializeField] private RewardUI _rewardUIPrefab;

    public void SetUp(List<Reward> rewards)
    {
        foreach(Reward reward in rewards)
        {
            RewardUI rewardUI = Instantiate(_rewardUIPrefab, _rewardItems);
            rewardUI.SetUp(reward);
        }

        _wrapper.SetActive(true);

        LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);

    }
}
