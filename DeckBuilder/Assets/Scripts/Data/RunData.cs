using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RunData
{
    [field: SerializeField] public List<CardData> Deck { get; private set; }
    [field: SerializeField] public HeroData Hero { get; private set; }
    [field: SerializeField] public Map Map { get; private set; }
    [field: SerializeField] public Room Room { get; private set; }
    public bool RoomCompleted => Room.IsCompleted;
    [field: SerializeField] public int CurrentHealth { get; private set; }
    [field: SerializeField] public int MaxHealth { get; private set; }
    [field: SerializeField] public int Credits { get; private set; } = 0;
    [field: SerializeField] public List<Perk> Perks { get; private set; } = new();
    public List<PerkData> UsedPerks { get; private set; } = new();

    public RunData(HeroData heroData)
    {
        Hero = heroData;
        Deck = new(Hero.StartingDeck);
        MaxHealth = Hero.StartingMaxHealth;
        CurrentHealth = Hero.StartingMaxHealth;
    }

    public string ToJson()
    {
        string json = JsonUtility.ToJson(this);

        return json;
    }

    public static RunData FromJson(string json)
    {
        RunData runData = JsonUtility.FromJson<RunData>(json);

        return runData;
    }

    public void EnterRoom(Room room)
    {
        Room = room;
        RNG.SetSeed(room.Seed);
        room.Enter();
    }

    public void SetCredits(int credits)
    {
        Credits = credits;
    }

    public void SetMap(Map map)
    {
        Map = map;
    }

    public void SetHealth(int health)
    {
        CurrentHealth = health;
    }

    public void MarkPerkDataUsed(PerkData perkData)
    {
        UsedPerks.Add(perkData);
    }
}
