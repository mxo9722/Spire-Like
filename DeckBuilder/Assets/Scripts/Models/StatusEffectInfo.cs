using System;
using UnityEngine;

[Serializable]
public class StatusEffectInfo
{
    [SerializeField] private string _name;
    public string Name { get => _name; }
    [SerializeField] private Sprite _sprite;
    public Sprite Sprite { get => _sprite; }

    [TextArea(2,4)]
    [SerializeField] private string _description;
    public string Description { get => _description; }

}
