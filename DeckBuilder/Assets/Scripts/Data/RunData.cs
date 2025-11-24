using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RunData
{
    [field: SerializeField] public List<CardData> Deck { get; private set; }
    [field: SerializeField] public HeroData Hero { get; private set; }

    [field: SerializeField] public Room Room { get; private set; }
    [field: SerializeField] public bool RoomCompleted => Room.IsCompleted;
    //TODO: Add code for Map when that's around
    [field: SerializeField] public int CurrentHealth { get; private set; }
    [field: SerializeField] public int MaxHealth { get; private set; }
    [field: SerializeField] public int Credits { get; private set; } = 0;
    [field: SerializeField] public List<Perk> Perks { get; private set; } = new();
    [field: SerializeField] public int RandomSeed { get; private set; }

    public RunData(HeroData heroData)
    {
        Hero = heroData;
        Deck = new(Hero.StartingDeck);
        MaxHealth = Hero.StartingMaxHealth;
        CurrentHealth = Hero.StartingMaxHealth;
        RandomSeed = RNG.Random.Next();
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

    public void SetRoom(Room room)
    {
        Room = room;
        RNG.SetSeed(room.Seed);
    }

    public void AddCredits(int amount)
    {
        Credits += amount;
    }
}
