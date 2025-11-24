using UnityEngine;
using UnityEngine.UI;

public class RewardUI : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private Image _image;
    [SerializeField] private TMPro.TMP_Text _text;

    private Reward _rewardable;

    public void SetUp(Reward rewardable)
    {
        _rewardable = rewardable;

        _image.sprite = rewardable.RewardImage;
        _text.text = rewardable.RewardName;
    }

    public void CollectReward()
    {
        if(_rewardable != null)
            _rewardable.CollectReward();

        CombatRoom combatRoom = (CombatRoom)RunSystem.Instance.GetRoom();

        combatRoom.Rewards.Remove(_rewardable);

        RunSystem.Instance.SaveRun();

        Destroy(gameObject);
    }
}
