using System;
using UnityEngine;

public class KarmaSystem : PersistentSingleton<KarmaSystem>
{
    public Action<int> OnKarmaChange;

    public int Karma { get; private set; }

    private void Start()
    {
        Karma = RunSystem.Instance.RunData.Karma;
    }

    public void Add(int amount)
    {
        Karma += amount;

        UpdateUI();
        OnKarmaChange?.Invoke(Karma);
    }

    public bool TrySpend(int amount)
    {
        if (Karma < amount)
            return false;

        Karma -= amount;
        UpdateUI();
        OnKarmaChange?.Invoke(Karma);

        return true;
    }
    
    public void RemoveCredits(int amount)
    {
        Karma = (int)MathF.Max(Karma - amount, 0);
        UpdateUI();
        OnKarmaChange?.Invoke(Karma);
    }

    public void UpdateUI()
    {
        TopBarUI.Instance.UpdateCredits(Karma);
    }
}
