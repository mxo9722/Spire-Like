using System.Collections.Generic;
using UnityEngine;

public class HealUnitsGA : GameAction
{
    public List<CombatantView> Targets { get; private set; }
    public int Amount { get; private set; }

    public HealUnitsGA(List<CombatantView> targets, int amount)
    {
        Targets = targets;
        Amount = amount;
    }
}
