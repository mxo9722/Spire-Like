using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Hero
{
    public HeroData Data { get; private set; }

    public List<CardData> StartingDeck => Data.StartingDeck;
    public List<CardData> ClassCards => Data.GetClassCards();
    public List<PerkData> Perks => Data.GetClassPerks();
    public int StartingMaxHealth => Data.StartingMaxHealth;
    public Sprite Image => Data.Image;



    public int CurrentHealth { get; private set; }
    public int MaxHealth { get; private set; }

    public Hero(HeroData heroData)
    {
        Data = heroData;

        CurrentHealth = heroData.StartingMaxHealth;
        MaxHealth = heroData.StartingMaxHealth;
    }

    public void SetCurrentHealth(int health)
    {
        CurrentHealth = health;
    }
    
    public void SetMaxHealth(int health)
    {
        MaxHealth = health;
    }
}
