using System.Collections.Generic;
using UnityEngine;

public class RunSystem : PersistentSingleton<RunSystem>, IHoldData
{
    [SerializeField] private HeroData _defaultHeroData1;
    [SerializeField] private HeroData _defaultHeroData2;
    
    public RunData RunData { get; private set; }

    [SerializeField] private bool _debugStartNew = false;

    private const string SaveKey = "Run1";

    public Hero Hero1 => RunData.Hero1;
    public Hero Hero2 => RunData.Hero2;

    public List<Card> Deck => RunData.Deck;
    public List<Perk> Perks => RunData.Perks;
    public List<PerkData> UsedPerks => RunData.UsedPerks;

    private bool _started = false;

    protected override void Awake()
    {
        base.Awake();
        if(Instance == this)
            LoadRun();
    }

    private void Start()
    {
        RenownSystem.Instance.OnRenownChange += RunData.SetCredits;
        _started = true;
    }

    private void OnEnable()
    {
        if(_started)
            RenownSystem.Instance.OnRenownChange += RunData.SetCredits;
    }

    private void OnDisable()
    {
        if (Instance == this)
        {
            RenownSystem.Instance.OnRenownChange -= RunData.SetCredits;
        }
    }

    public void SetUp(RunData runData)
    {
        RunData = runData;
    }

    public void LoadRun()
    {
        string key = SaveKey;

        if (PlayerPrefs.HasKey(key) && !_debugStartNew)
        {
            string json = PlayerPrefs.GetString(key);
            RunData runData = RunData.FromJson(json);
            RunData = runData;
        }
        else
        {
            RunData = GenerateNewRun();
        }
    }

    public void SaveRun()
    {
        if(RunData != null)
        {
            PlayerPrefs.SetString(SaveKey,RunData.ToJson());
        }
    }

    public RunData GenerateNewRun()
    {
        RunData runData = new(_defaultHeroData1, _defaultHeroData2);

        return runData;
    }

    public void AddPerk(Perk perk)
    {
        RunData.Perks.Add(perk);
        SaveRun();
    }

    public void AddCard(Card card)
    {
        RunData.Deck.Add(card);
        SaveRun();
    }

    public void RemoveCard(Card card)
    {
        Deck.Remove(card);
        SaveRun();
    }

    public void UpgradeCard(Card card)
    {
        int index = Deck.IndexOf(card);

        if (index == -1)
            return;

        Deck[index] = new(card.Upgrade, card.Owner);
    }

    public Room GetRoom() => RunData.Room;
    public void SetMap(Map map) => RunData.SetMap(map);
    public void MarkPerkUsed(PerkData perkData) => RunData.MarkPerkDataUsed(perkData);

    public void AddData(string key, object data) => RunData.AddData(key, data);
    public T GetData<T>(string key) => RunData.GetData<T>(key);
    public bool ContainsKey(string key) => RunData.ContainsKey(key);
}
