using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PerksUI : MonoBehaviour
{
    [SerializeField] private PerkUI _perkUIPrefab;

    private readonly List<PerkUI> _perkUIs = new();

    public void AddPerkUI(Perk perk)
    {
        PerkUI perkUI = Instantiate(_perkUIPrefab, transform);
        perkUI.SetUp(perk);
        _perkUIs.Add(perkUI);
    }

    public void RemovePerkUI(Perk perk)
    {
        PerkUI perkUI = _perkUIs.Where(pui => pui.Perk == perk).FirstOrDefault();
        if(perkUI != null)
        {
            _perkUIs.Remove(perkUI);
            Destroy(perkUI.gameObject);
        }
    }
}
