using UnityEngine;
using UnityEngine.UI;

public class PerkUI : MonoBehaviour
{
    [SerializeField] private Image _image;
    public Perk Perk { get; private set; }

    public void SetUp(Perk perk)
    {
        Perk = perk;
        _image.sprite = perk.Image;
    }
}
