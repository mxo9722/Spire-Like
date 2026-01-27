using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardsUI : MonoBehaviour
{
    [SerializeField] private Transform _rewardItems;
    [SerializeField] private GameObject _wrapper;
    [SerializeField] private RewardUI _rewardUIPrefab;

    public bool IsOpen { get => _wrapper.activeSelf; }

    private List<SetReward> _rewards = new();
    private List<RewardUI> _rewardUIs = new();
    private Action _onClose;

    public void SetUp(List<SetReward> rewards, Action onClose)
    {
        if (rewards.Count > 0)
        {
            _rewards = new();
            _rewardUIs.Clear();

            foreach (Reward reward in rewards)
            {
                reward.SetUp();

                SetReward setReward = reward.GetSetReward();

                RewardUI rewardUI = Instantiate(_rewardUIPrefab, _rewardItems);
                rewardUI.SetUp(setReward);
                _rewards.Add(setReward);

                _rewardUIs.Add(rewardUI);
            }

            _wrapper.SetActive(true);
            _rewardItems.gameObject.SetActive(false);
            _rewardItems.gameObject.SetActive(true);

            LayoutRebuilder.ForceRebuildLayoutImmediate(_rewardItems as RectTransform);
            _onClose = onClose;
        }
        else
        {
            onClose.Invoke();
        }
    }

    public void RemoveReward(SetReward reward)
    {
        _rewards.Remove(reward);
        RewardUI rewardUI = _rewardUIs.Find(i => i.Reward == reward);
        _rewardUIs.Remove(rewardUI);
        Destroy(rewardUI.gameObject);


        _rewardItems.gameObject.SetActive(false);
        _rewardItems.gameObject.SetActive(true);

        LayoutRebuilder.ForceRebuildLayoutImmediate(_rewardItems as RectTransform);
    }

    public void Show()
    {
        _wrapper.SetActive(true);
        _rewardItems.gameObject.SetActive(false);
        _rewardItems.gameObject.SetActive(true);

        LayoutRebuilder.ForceRebuildLayoutImmediate(_rewardItems as RectTransform);
    }
    
    public void Hide()
    {
        _wrapper.SetActive(false);
    }

    public void Close()
    {
        Hide();

        foreach (RewardUI rewardUI in _rewardUIs)
        {
            Destroy(rewardUI.gameObject);
        }

        _rewardUIs.Clear();

        _onClose?.Invoke();
        _onClose = null;
    }
}
