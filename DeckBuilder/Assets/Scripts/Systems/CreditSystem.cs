using System;
using UnityEngine;

public class CreditSystem : PersistentSingleton<CreditSystem>
{
    public Action<int> OnCreditsChange;

    public int Credits { get; private set; }

    private void Start()
    {
        Credits = RunSystem.Instance.RunData.Credits;
    }

    public void Add(int amount)
    {
        Credits += amount;

        UpdateUI();
        OnCreditsChange?.Invoke(Credits);
    }

    public bool TrySpend(int amount)
    {
        if (Credits < amount)
            return false;

        Credits -= amount;
        UpdateUI();
        OnCreditsChange?.Invoke(Credits);

        return true;
    }
    
    public void RemoveCredits(int amount)
    {
        Credits = (int)MathF.Max(Credits - amount, 0);
        UpdateUI();
        OnCreditsChange?.Invoke(Credits);
    }

    public void UpdateUI()
    {
        TopBarUI.Instance.UpdateCredits(Credits);
    }
}
