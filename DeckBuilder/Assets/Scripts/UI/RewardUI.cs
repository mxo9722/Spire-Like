using UnityEngine;
using UnityEngine.UI;

public class RewardUI : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private Image _image;
    [SerializeField] private TMPro.TMP_Text _text;

    public SetReward Reward { get; private set; }

    public void SetUp(SetReward rewardable)
    {
        Reward = rewardable;

        _image.sprite = rewardable.RewardImage;
        _text.text = rewardable.RewardName;
    }

    public void CollectReward()
    {
        if(Reward != null)
            Reward.CollectReward();

        Room room = RunSystem.Instance.GetRoom();

        if(room is IHaveRewards rewardsRoom)
        {
            rewardsRoom.RemoveReward(Reward);
        }

        RunSystem.Instance.SaveRun();
    }
}
