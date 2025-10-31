using System.Collections.Generic;
using UnityEngine;

public class PerkSystem : Singleton<PerkSystem>
{
    [SerializeField] private PerksUI _perksUI;

    public readonly List<Perk> perks = new();

    public void AddPerk(Perk perk)
    {
        perks.Add(perk);
        _perksUI.AddPerkUI(perk);
        perk.OnAdd();
    }

    public void RemovePerk(Perk perk)
    {
        perks.Remove(perk);
        _perksUI.RemovePerkUI(perk);
        perk.OnRemove();
    }
}
