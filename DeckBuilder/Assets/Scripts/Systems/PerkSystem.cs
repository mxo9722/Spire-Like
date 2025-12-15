using System.Collections.Generic;
using UnityEngine;

public class PerkSystem : Singleton<PerkSystem>
{
    [SerializeField] private PerksUI _perksUI;

    public List<Perk> Perks { get; private set; } = new();

    private void Start()
    {
        Perks = RunSystem.Instance.Perks;

        foreach (Perk perk in Perks)
            AddPerkView(perk);
    }

    public void ObtainPerk(PerkData perkData)
    {
        Perk perk = new(perkData);

        Perks.Add(perk);
        AddPerkView(perk);
        RunSystem.Instance.SaveRun();
    }

    private void AddPerkView(Perk perk)
    {
        _perksUI.AddPerkUI(perk);
        perk.OnAdd();
    }

    public void RemovePerk(Perk perk)
    {
        Perks.Remove(perk);
        _perksUI.RemovePerkUI(perk);
        perk.OnRemove();
    }
}
