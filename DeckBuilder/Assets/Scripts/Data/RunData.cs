using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class RunData : IHoldData
{
    [field: SerializeField] public List<Card> Deck { get; private set; }
    [field: SerializeField] public Hero Hero1 { get; private set; }
    [field: SerializeField] public Hero Hero2 { get; private set; }
    [field: SerializeField] public Map Map { get; private set; }
    [field: SerializeField] public Room Room { get; private set; }
    public bool RoomCompleted => Room.IsCompleted;
    [field: SerializeField] public int Renown { get; private set; } = 100;
    [field: SerializeField] public List<Perk> Perks { get; private set; } = new();
    public List<PerkData> UsedPerks { get; private set; } = new();


    private SerializedDictionary<string, object> _data = null;

    public RunData(HeroData heroData1, HeroData heroData2, List<CardData> colorlessCards)
    {
        Hero1 = new(heroData1);
        Hero2 = new(heroData2);

        Deck = colorlessCards.Select(c => new Card(c, null)).ToList();
        Deck.AddRange(Hero1.StartingDeck.Select(c => new Card(c, Hero1.Data)));
        Deck.AddRange(Hero2.StartingDeck.Select(c => new Card(c, Hero2.Data)));
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
        Renown = credits;
    }

    public void SetMap(Map map)
    {
        Map = map;
    }

    public void MarkPerkDataUsed(PerkData perkData)
    {
        UsedPerks.Add(perkData);
    }

    public void AddData(string key, object data)
    {
        if (_data == null)
            _data = new();

        if (_data.ContainsKey(key))
            _data[key] = data;
        else
            _data.Add(key, data);
    }

    public T GetData<T>(string key)
    {
        if (_data == null || !_data.ContainsKey(key))
            return default(T);

        if (_data[key] is T t)
            return t;

        return default(T);
    }

    public bool ContainsKey(string key)
    {
        if (_data == null)
            return false;

        return _data.ContainsKey(key);
    }
}
