using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusEffectUI : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private TMP_Text _stackCountText;

    public void Set(Sprite sprite, int stackCount)
    {
        _image.sprite = sprite;
        _stackCountText.text = stackCount.ToString();
    }
}
