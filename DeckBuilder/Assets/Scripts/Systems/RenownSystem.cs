using System;
using UnityEngine;

public class RenownSystem : PersistentSingleton<RenownSystem>
{
    public Action<int> OnRenownChange;

    public int Renown { get; private set; }

    private void Start()
    {
        Renown = RunSystem.Instance.RunData.Renown;
    }

    public void Add(int amount)
    {
        Renown += amount;

        UpdateUI();
        OnRenownChange?.Invoke(Renown);
    }

    public bool TrySpend(int amount)
    {
        if (Renown < amount)
            return false;

        Renown -= amount;
        UpdateUI();
        OnRenownChange?.Invoke(Renown);

        return true;
    }
    
    public void RemoveRenown(int amount)
    {
        Renown = Renown - amount;
        UpdateUI();
        OnRenownChange?.Invoke(Renown);
    }

    public void UpdateUI()
    {
        TopBarUI.Instance.UpdateRenown(Renown);
    }
}
