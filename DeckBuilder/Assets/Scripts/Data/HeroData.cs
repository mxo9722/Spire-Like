using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HeroData", menuName = "Data/Hero")]
public class HeroData : ScriptableObject
{
    [field:SerializeField] public string Name { get; private set; }
    [field:SerializeField] public Sprite Image { get; private set; }
    [field:SerializeField] public int Health { get; private set; }
    [field:SerializeField] public List<CardData> Deck { get; private set; }
}
