using UnityEngine;

public class RunSystem : PersistentSingleton<RunSystem>
{
    [SerializeField] private HeroData _defaultHeroData;
    
    public RunData RunData { get; private set; }

    [SerializeField] private bool _debugStartNew = false;

    private const string SaveKey = "Run1";

    protected override void Awake()
    {
        base.Awake();
        if(Instance == this)
            LoadRun();
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
            RunData = new(_defaultHeroData);
        }

        RNG.SetSeed(RunData.RandomSeed);
    }

    public void SaveRun()
    {
        if(RunData != null)
        {
            PlayerPrefs.SetString(SaveKey,RunData.ToJson());
        }
    }

    public Room GetRoom()
    {
        return RunData.Room;
    }

    public void AddPerk(PerkData perk)
    {
        RunData.Perks.Add(new(perk));
    }

    public void AddCredit(int amount) => RunData.AddCredits(amount);
}
