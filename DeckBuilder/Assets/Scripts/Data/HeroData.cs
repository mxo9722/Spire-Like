using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HeroData", menuName = "Data/Hero")]
public class HeroData : ScriptableObject
{
    [field:SerializeField] public string Name { get; private set; }
    [field:SerializeField] public Sprite Image { get; private set; }
    [field:SerializeField] public int StartingMaxHealth { get; private set; }
    [field:SerializeField] public List<CardData> StartingDeck { get; private set; }
    [field:SerializeField] public PerkData StartingPerk { get; private set; }
    [field:SerializeField] public Color Color { get; private set; }

    [SerializeField] private List<CardData> _allCards;
    [SerializeField] private List<PerkData> _perks;

    public List<CardData> GetClassCards()
    {
        return new(_allCards);
    }
    
    public List<PerkData> GetClassPerks()
    {
        return new(_perks);
    }
}
