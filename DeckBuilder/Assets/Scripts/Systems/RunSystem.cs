using System.Collections.Generic;
using UnityEngine;

public class RunSystem : PersistentSingleton<RunSystem>
{
    [SerializeField] private HeroData _defaultHeroData;
    
    public RunData RunData { get; private set; }

    [SerializeField] private bool _debugStartNew = false;

    private const string SaveKey = "Run1";
    public int CurrentHealth => RunData.CurrentHealth;
    public int MaxHealth => RunData.MaxHealth;
    public List<CardData> Deck => RunData.Deck;
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
        CreditSystem.Instance.OnCreditsChange += RunData.SetCredits;
        _started = true;
    }

    private void OnEnable()
    {
        if(_started)
            CreditSystem.Instance.OnCreditsChange += RunData.SetCredits;
    }

    private void OnDisable()
    {
        if (Instance == this)
        {
            CreditSystem.Instance.OnCreditsChange -= RunData.SetCredits;
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
        RunData runData = new(_defaultHeroData);

        return runData;
    }

    public void AddPerk(Perk perk)
    {
        RunData.Perks.Add(perk);
        SaveRun();
    }

    public void AddCard(Card card)
    {
        RunData.Deck.Add(card.data);
        SaveRun();
    }

    public void RemoveCard(Card card)
    {
        Deck.Remove(card.data);
        SaveRun();
    }

    public void UpgradeCard(CardData card)
    {
        int index = Deck.IndexOf(card);

        if (index == -1)
            return;

        Deck[index] = card.Upgrade;
    }

    public Room GetRoom() => RunData.Room;
    public HeroData GetHeroData() => RunData.Hero;
    public void SetMap(Map map) => RunData.SetMap(map);
    public void MarkPerkUsed(PerkData perkData) => RunData.MarkPerkDataUsed(perkData);
    public void SetHealth(int amount) => RunData.SetHealth(amount);
}
