using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RewardUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Button _button;
    [SerializeField] private Image _image;
    [SerializeField] private TMPro.TMP_Text _text;
    [SerializeField] private HelpBoxUI _helpBox;

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

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(Reward.ShowTip)
            _helpBox.SetUpFromText(Reward.RewardName, Reward.RewardDescription);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(Reward.ShowTip)
            _helpBox.Hide();
    }
}
